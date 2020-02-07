module Layout exposing (viewMain)

import Types exposing (..)
import Html exposing (Html)
import Element exposing (..)
import Element.Background as Background
import Element.Border as Border
import Element.Events exposing (..)
import Element.Font as Font
import Element.Input as Input
import Element.Region as Region


viewMain : Model -> Html Msg
viewMain model =
    Element.layout
        [ Font.size 20
        ]
    <|
        column [centerX, width fill, spacing 10]
            [
             header
            , spacerLine
            , case model of
                  Loading -> loadingView
                  KickerToolRunning -> kickerToolRunningView
                  Turnering t -> turnamentView t
                  Failure prob ->  case prob of
                                       LoadingError -> text "loading error"
                                       ParsingError s -> text s

            ]


-- HELPERS

turnamentView : Turnament ->  Element Msg
turnamentView t  =
    -- if turnament is not finished handle it

    -- turnament is ready to be set up

    -- Add User Button

    -- add team
    column
    [centerX]
    [
     Input.text
         [ centerX]
         { label = Input.labelLeft [] (text "Spiller 1")
         , onChange = \name -> LoadPlayer1Suggestions name
         , placeholder = Nothing -- Just (Input.placeholder [] (text placeholder)) -- only is text is empty
         , text = t.name1 -- user number not name
         }
    , Input.text
        [ centerX]
        { label = Input.labelLeft [] (text "Spiller 2")
        , onChange = \name -> LoadPlayer1Suggestions name
        , placeholder = Nothing -- Just (Input.placeholder [] (text placeholder)) -- only is text is empty
        , text = t.name2 -- user number not name
        }

    , if True -- TODO only show when both players has a phone number in them
      then
          Input.button
              buttonLayout
              { onPress = Just (AddTeam "s1" "s2")
              , label = text "Gem"
              }
      else
          Element.none
    ]

    --


    -- List of teams

    -- tables
    -- groups
    -- point





kickerToolRunningView : Element Msg
kickerToolRunningView =
    el
    [
     centerX
    , centerY
    , Font.size 20
    , padding 20
    ]
    (text "Kicker tool køre luk det for at starte turnering")


loadingView : Element Msg
loadingView =
    el
    [
     centerX
    , centerY
    , Font.size 20
    , padding 20
    ]
    (text "Finder info fra kicker tool")

header : Element Msg
header =
    el
    [ Region.heading 1
    , centerX
    , centerY
    , Font.size 36
    , padding 20
    ]
    (text "Turnering system med kicker tool")

spacerLine : Element Msg
spacerLine =
    column [width fill]
        [
         el [ width fill
            , Border.width 1
            , Border.color gray
            ]
             none
        , el [ paddingXY 0 10 ] none
        ]


buttonLayout : (List (Attribute Msg))
buttonLayout = [ Background.color buttonColor
               , Font.color black
               , Border.color black
               , paddingXY 16 10
               , Border.rounded 3
               , Font.size 20
               ]


buttonColor = darkGray

panelBackgroundColor = gray


white = Element.rgb 1 1 1

black = Element.rgb 0  0 0

gray = Element.rgb 0.9 0.9 0.9

darkGray = Element.rgb 0.8 0.8 0.8

blue = Element.rgb 0 0 0.8

red = Element.rgb 0.8 0 0

darkBlue = Element.rgb 0 0 0.9
