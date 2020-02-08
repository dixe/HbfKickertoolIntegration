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

-- MAIN
main =
    Browser.element { init = init, update = update, subscriptions =  subscriptions, view = view }

init : () -> (Model, Cmd Msg)
init _ = (Turnering emptyTournament, Cmd.none)


-- UPDATE


update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
    case msg of
        Load -> (Loading, Cmd.none) -- cmd http request load
        LoadPlayer1Suggestions name -> (updatePlayer1Name model name, loadPlayer name GotPlayer1Suggestions)
        LoadPlayer2Suggestions name -> (updatePlayer2Name model name, loadPlayer name GotPlayer2Suggestions)
        AddTeam number1 number2 -> (model, addTeamGet number1 number2)
        GotTeam result -> fromResult result teamDecoder (addTeam model)
        GotPlayer1Suggestions result -> fromResult result playerSuggestionsDecoder (updatePlayerSuggestions model) -- cmd http request load
        GotPlayer2Suggestions result -> fromResult result playerSuggestionsDecoder (updatePlayerSuggestions model) -- cmd http request load
        StartTournament -> (model, case model of
                                      Turnering t -> startTournament t TournamentStarted
                                      _ -> Cmd.none)
        TournamentStarted _ -> (model, Cmd.none)


updatePlayer1Name : Model -> String -> Model
updatePlayer1Name model name =
    case model of
        Turnering t -> Turnering {t | name1 = name}
        _ -> model


updatePlayer2Name : Model -> String -> Model
updatePlayer2Name model name =
    case model of
        Turnering t -> Turnering {t | name2 = name}
        _ -> model


updatePlayerSuggestions : Model -> List String -> Model
updatePlayerSuggestions model names =
    case model of
        Turnering t -> Turnering {t | playerSuggestions = names}
        _ -> model

addTeam : Model -> Team -> Model
addTeam model team =
    case model of
        Turnering t -> Turnering {t | teams = t.teams ++ [team], name1 = "", name2 = ""}
        _ -> model

fromResult : (Result Http.Error String) -> Decode.Decoder a -> (a -> Model) -> (Model, Cmd Msg)
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
