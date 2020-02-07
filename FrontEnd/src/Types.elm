module Types exposing (..)

import Http

type Msg = Load
         | LoadPlayer1Suggestions String
         | AddTeam String String
         | GotTeam (Result Http.Error String)
         | GotPlayer1Suggestions (Result Http.Error String)

type Model = Loading | KickerToolRunning | Turnering Turnament | Failure Problem

type Problem = LoadingError | ParsingError String

type alias Turnament = { teams : List String
                       , number1 : String
                       , number2 : String
                       , name1 : String
                       , name2 : String
                       , playerSuggestions : List String
                       }
