namespace HelloMediaElement

open Fabulous
open Fabulous.Maui
open Fabulous.MauiControls.MediaElement
open Microsoft.Maui
open Microsoft.Maui.Accessibility

open type Fabulous.Maui.View

module App =
    type Model = { Count: int }

    type Msg = | Clicked

    type CmdMsg = SemanticAnnounce of string

    let semanticAnnounce text =
        Cmd.ofSub(fun _ -> SemanticScreenReader.Announce(text))

    let mapCmd cmdMsg =
        match cmdMsg with
        | SemanticAnnounce text -> semanticAnnounce text

    let init () = { Count = 0 }, []

    let update msg model =
        match msg with
        | Clicked -> { model with Count = model.Count + 1 }, [ SemanticAnnounce $"Clicked {model.Count} times" ]

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

                        MediaElement("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4")
                            .heightRequest(300)
                            .widthRequest(400)
                        
                        let text =
                            if model.Count = 0 then
                                "Click me"
                            else
                                $"Clicked {model.Count} times"

                        Button(text, Clicked)
                            .semantics(hint = "Counts the number of times you click")
                            .centerHorizontal()
                    })
                        .padding(Thickness(30., 0., 30., 0.))
                        .centerVertical()
                )
            )
        )

    let program = Program.statefulWithCmdMsg init update view mapCmd