module Main exposing (..)

import Browser
import Html exposing (..)
import Http
import Json.Decode as Decode exposing (Decoder, field, map2, int, string, decodeString, errorToString, list) -- maybe not in type module
import Json.Decode.Pipeline exposing (required, optional, hardcoded)
import List
import Types exposing (..)
import Layout exposing (viewMain)

-- MAIN
main =
    Browser.element { init = init, update = update, subscriptions =  subscriptions, view = view }

init : () -> (Model, Cmd Msg)
init _ = (Turnering {teams = [],  number1 = "", number2 = "", name1 = "", name2 = "", playerSuggestions = []}, Cmd.none)


-- UPDATE


update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
    case msg of
        Load -> (Loading, Cmd.none) -- cmd http request load
        LoadPlayer1Suggestions name -> (updatePlayer1Name model name, loadPlayer name GotPlayer1Suggestions) -- cmd http request load
        AddTeam number1 number2 -> (model, addTeamGet number1 number2)
        GotTeam result -> fromResult result teamDecoder (addTeam model)
        GotPlayer1Suggestions result -> fromResult result playerSuggestionsDecoder (updatePlayerSuggestions model) -- cmd http request load



updatePlayer1Name : Model -> String -> Model
updatePlayer1Name model name =
    case model of
        Turnering t -> Turnering {t | name1 = name}
        _ -> model


updatePlayerSuggestions : Model -> List String -> Model
updatePlayerSuggestions model names =
    case model of
        Turnering t -> Turnering {t | playerSuggestions = names}
        _ -> model

addTeam : Model -> String -> Model
addTeam model team =
    case model of
        Turnering t -> Turnering {t | teams = t.teams ++ [team]}
        _ -> model

fromResult : (Result Http.Error String) -> Decoder a -> (a -> Model) -> (Model, Cmd Msg)
fromResult result decoder ret  =
    case result of
        Ok allText ->
            case decodeString decoder allText of
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
loadPlayer name msg=
    Http.get
        { url = String.join "" ["http://localhost:49979/api/Users/GetPlayerSuggestions?name="
                               , name
                               ]
        , expect = Http.expectString msg
        }


teamDecoder : Decoder String
teamDecoder =
  field "data" (field "image_url" string)



playerSuggestionDecoder : Decoder String
playerSuggestionDecoder =
    field "name" string

playerSuggestionsDecoder : Decoder (List String)
playerSuggestionsDecoder =
  field "suggestions" ( list playerSuggestionDecoder)
