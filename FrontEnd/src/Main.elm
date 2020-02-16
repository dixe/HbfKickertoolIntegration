module Main exposing (..)


import Browser
import Html exposing (..)
import Http
--import Json.Decode as Decode exposing (Decoder, field, map2, map4, int, string, decodeString, errorToString, list) -- maybe not in type module
import Json.Encode as Encode
import Json.Decode as Decode
import List
import Types exposing (..)
import Layout exposing (viewMain)
import Select



-- MAIN
main =
    Browser.element { init = init, update = update, subscriptions =  subscriptions, view = view }

init : () -> (Model, Cmd Msg)
init _ = (initModel, Cmd.none)


initModel : Model
initModel = { selectState = Select.newState "p1"
            , modelState = Turnering emptyTournament
            , focused = None
            }

-- UPDATE


update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
    case msg of
        Load -> ({model | modelState = Loading}, Cmd.none) -- cmd http request load
        LoadPlayer1Suggestions name -> (updateModelState {model | focused = Player1} updatePlayer1Name name, loadPlayer name GotPlayer1Suggestions)
        LoadPlayer2Suggestions name -> (updateModelState {model | focused = Player2} updatePlayer2Name name, loadPlayer name GotPlayer2Suggestions)
        AddTeam number1 number2 -> (model, addTeamGet number1 number2)
        GotTeam result ->
            let (state,cmd) = fromResult result teamDecoder (addTeam model.modelState)
            in ({model | modelState = state}, cmd)
        GotPlayer1Suggestions result -> let (state,cmd) = fromResult result playerSuggestionsDecoder (updatePlayerSuggestions model.modelState) -- cmd http request load
                                        in ({model | modelState = state}, cmd)
        GotPlayer2Suggestions result -> olet (state,cmd) = fromResult result playerSuggestionsDecoder (updatePlayerSuggestions model.modelState) -- cmd http request load
                                        in ({model | modelState = state}, cmd)
        StartTournament -> (model,
                                case model.modelState of
                                    Turnering t -> startTournament t TournamentStarted
                                    _ -> Cmd.none)
        TournamentStarted _ -> (model, Cmd.none)
        OnSelect maybeName ->
            let
                name = case maybeName of
                           Just n -> n
                           Nothing -> ""
            in
                (updateModelState model updatePlayer1Name name, Cmd.none)
        SelectMsg name subMsg ->
            let
                (state, cmd) = (updatePlayer1Name model.modelState name, loadPlayer name GotPlayer1Suggestions)
                (a, _) =
                    Select.update selectConfig subMsg model.selectState
            in
                ( {model | selectState = a, modelState = state}, cmd)


updateModelState : Model -> (ModelState -> a -> ModelState) -> a -> Model
updateModelState m f arg
    = { m | modelState = f m.modelState arg}


updatePlayer1Name : ModelState -> String -> ModelState
updatePlayer1Name model name =
    case model of
        Turnering t -> Turnering {t | name1 = name}
        _ -> model


updatePlayer2Name : ModelState -> String -> ModelState
updatePlayer2Name model name =
    case model of
        Turnering t -> Turnering {t | name2 = name}
        _ -> model


updatePlayerSuggestions : ModelState -> List String -> ModelState
updatePlayerSuggestions model names =
    case model of
        Turnering t -> Turnering {t | playerSuggestions = names}
        _ -> model

addTeam : ModelState -> Team -> ModelState
addTeam model team =
    case model of
        Turnering t -> Turnering {t | teams = t.teams ++ [team], name1 = "", name2 = ""}
        _ -> model

fromResult : (Result Http.Error String) -> Decode.Decoder a -> (a -> ModelState) -> (ModelState, Cmd Msg)
fromResult result decoder ret  =
    case result of
        Ok allText ->
            case Decode.decodeString decoder allText of
                Ok decoded ->
                    (ret decoded, Cmd.none)
                Err r ->
                    (Failure (ParsingError (Decode.errorToString r)), Cmd.none)
        Err _ ->
            (Failure LoadingError, Cmd.none)


-- SUBSCRIPTIONS

subscriptions : Model -> Sub Msg
subscriptions model = Sub.none

-- VIEW
view : Model -> Html Msg
view model = viewMain model


-- HTTP

-- Get the names of the players from the phone numbers
addTeamGet : String -> String -> Cmd Msg
addTeamGet number1 number2 =
    Http.get
        { url = String.join "" ["http://localhost:49979/api/Users/GetTeam?number1="
                               ,number1
                               ,"&number2="
                               , number2
                               ]
        , expect = Http.expectString GotTeam
        }



-- Get the names of the players from the phone numbers
loadPlayer : String -> (Result Http.Error String -> Msg) -> Cmd Msg
loadPlayer name msg =
    Http.get
        { url = String.join "" ["http://localhost:49979/api/Users/GetPlayerSuggestions?name="
                               , name
                               ]
        , expect = Http.expectString msg
        }


startTournament : Tournament -> (Result Http.Error String -> Msg) -> Cmd Msg
startTournament t msg =
    Http.post
        { url = "http://localhost:49979/api/KickerTournament/GenerateNewTournament"
        , body = Http.jsonBody (encodeTournament t)
        , expect = Http.expectString msg
        }





teamDecoder : Decode.Decoder Team
teamDecoder =
    Decode.map4 Team
        (Decode.field "player1Name" Decode.string)
        (Decode.field "player2Name" Decode.string)
        (Decode.field "player1Number" Decode.string)
        (Decode.field "player2Number" Decode.string)




playerSuggestionDecoder : Decode.Decoder String
playerSuggestionDecoder =
    Decode.field "name" Decode.string

playerSuggestionsDecoder : Decode.Decoder (List String)
playerSuggestionsDecoder =
  Decode.field "suggestions" ( Decode.list playerSuggestionDecoder)


-- GENERATED JSON DECODERS


decodeTeam =
    Decode.map4
        Team
            ( Decode.field "player1Name" Decode.string )
            ( Decode.field "player2Name" Decode.string )
            ( Decode.field "player1Number" Decode.string )
            ( Decode.field "player2Number" Decode.string )

decodeTournament =
    Decode.map6
        Tournament
            ( Decode.field "teams" (Decode.list decodeTeam) )
            ( Decode.field "number1" Decode.string )
            ( Decode.field "number2" Decode.string )
            ( Decode.field "name1" Decode.string )
            ( Decode.field "name2" Decode.string )
            ( Decode.field "playerSuggestions" (Decode.list Decode.string) )

encodeTeam a =
    Encode.object
        [ ("player1Name", Encode.string a.player1Name)
        , ("player2Name", Encode.string a.player2Name)
        , ("player1Number", Encode.string a.player1Number)
        , ("player2Number", Encode.string a.player2Number)
        ]

encodeTournament a =
    Encode.object
        [ ("teams", Encode.list encodeTeam a.teams)
        , ("number1", Encode.string a.number1)
        , ("number2", Encode.string a.number2)
        , ("name1", Encode.string a.name1)
        , ("name2", Encode.string a.name2)
        , ("playerSuggestions", Encode.list Encode.string a.playerSuggestions)
        ]
