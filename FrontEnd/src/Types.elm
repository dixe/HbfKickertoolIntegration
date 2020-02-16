module Types exposing (..)

import Http

type Msg = Load
         | LoadPlayerSuggestions String Focused
         | AddTeam String String
         | GotTeam (Result Http.Error String)
         | GotPlayerSuggestions (Result Http.Error String)
         | StartTournament
         | TournamentStarted (Result Http.Error String)
         | ClearSuggestions

type alias Model = { modelState : ModelState, focused : Focused }

type ModelState = Loading | KickerToolRunning | Turnering Tournament | Failure Problem

type Problem = LoadingError | ParsingError String

type Focused = None | Player1 | Player2

type alias Tournament = { teams : List Team
                        , number1 : String
                        , number2 : String
                        , name1 : String
                        , name2 : String
                        , playerSuggestions : List String
                        , groups : Int
                        , tables : List Int
                        }

type alias Team =
    { player1Name: String
    , player2Name : String
    , player1Number : String
    , player2Number : String
    }


emptyTournament : Tournament
emptyTournament =
    { teams = []
    , number1 = ""
    , number2 = ""
    , name1 = ""
    , name2 = ""
    , playerSuggestions = []
    , groups = 1
    , tables = [1,2,3,4,5]
    }
