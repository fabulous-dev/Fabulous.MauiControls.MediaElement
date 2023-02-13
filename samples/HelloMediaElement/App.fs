namespace HelloMediaElement

open Fabulous
open Fabulous.Maui
open Fabulous.MauiControls.MediaElement
open Microsoft.Maui

open type Fabulous.Maui.View

module App =
    type Model = { LastEvent: string }

    type Msg =
        | MediaEnded
        | PositionChanged of string

    let init () = { LastEvent = "No events yet" }, []

    let update msg model =
        match msg with
        | MediaEnded -> { model with LastEvent = "Media Ended" }, Cmd.none
        | PositionChanged s -> { model with LastEvent = "Position Changed to " + s }, Cmd.none

    let view model =
        Application(
            ContentPage(
                ScrollView(
                    (VStack(spacing = 25.) {

                        Label("Media element proof of concept")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Label("Welcome to .NET Multi-platform App UI powered by Fabulous")
                            .semantics(SemanticHeadingLevel.Level2, "Welcome to dot net Multi platform App U I powered by Fabulous")
                            .font(size = 18.)
                            .centerTextHorizontal()

                        MediaElement()
                            .source("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4")
                            .height(300)
                            .width(400)
                            .shouldAutoPlay(true)
                            .onMediaEnded(MediaEnded)
                            .onPositionChanged(fun x -> PositionChanged (x.Position.ToString("c")))
                         
                        Label("Last Event: " + model.LastEvent)
                            .font(size = 14.)
                            .centerTextHorizontal()   
                            

                    })
                        .padding(Thickness(30., 0., 30., 0.))
                        .centerVertical()
                )
            )
        )

    let program = Program.statefulWithCmd init update view