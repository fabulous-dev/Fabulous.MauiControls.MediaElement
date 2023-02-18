namespace HelloMediaElement

open Fabulous
open Fabulous.Maui
open Fabulous.MauiControls.MediaElement
open Microsoft.Maui

open type Fabulous.Maui.View

module App =
    type Model = {
        IsAppLoaded: bool
        LastEvent: string
    }

    type Msg =
        | AppLoaded
        | MediaEnded
        | MediaOpened
        | PauseVideoRequested
        | PositionChanged of System.TimeSpan
        | StartVideoRequested
        | VideoPaused
        | VideoStarted

    let init () = { IsAppLoaded = false ; LastEvent = "No events yet" }, []

    let controller = MediaElementController()
    
    let pauseVideoCmd () = 
        async {
            do controller.DoPause()
            return VideoPaused
        }
        |> Cmd.ofAsyncMsg
        
    let startVideoCmd () = 
        async {
            do controller.DoPlay()
            return VideoStarted
        }
        |> Cmd.ofAsyncMsg

    
    let update msg model =
        match msg with
        | AppLoaded -> { model with IsAppLoaded = true }, Cmd.none
        | MediaEnded -> { model with LastEvent = "Media Ended" }, Cmd.none
        | MediaOpened -> { model with LastEvent = "Media Opened" }, Cmd.none
        | PauseVideoRequested -> model, pauseVideoCmd()
        | PositionChanged t -> { model with LastEvent = "Position Changed to " + t.ToString("c") }, Cmd.none
        | StartVideoRequested -> model, startVideoCmd()
        | VideoPaused -> model, Cmd.none
        | VideoStarted -> model, Cmd.none
        

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

                        if model.IsAppLoaded then
                          MediaElement()
                            .source("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4")
                            .height(300)
                            .width(400)
                            .shouldAutoPlay(true)
                            .onMediaOpened(MediaOpened)
                            .onMediaEnded(MediaEnded)
                            .onPositionChanged(fun x -> PositionChanged (x.Position))
                            // TODO: fix this - uncommenting the controller crashes the app at startup
                            // .controller(controller) 
                        
                        Label("Latest Event: " + model.LastEvent)
                            .font(size = 14.)
                            .centerTextHorizontal()
                            
                        Button("Start video", StartVideoRequested)
                        Button("Pause video", PauseVideoRequested)
                            

                    })
                        .padding(Thickness(30., 0., 30., 0.))
                        .centerVertical()
                )
            )
        ).onStart(AppLoaded)

    let program = Program.statefulWithCmd init update view
