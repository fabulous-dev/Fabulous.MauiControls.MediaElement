namespace Fabulous.MauiControls.MediaElement

open System.Runtime.CompilerServices
open CommunityToolkit.Maui.Core.Primitives
open CommunityToolkit.Maui.Views
open Fabulous
open Fabulous.StackAllocatedCollections.StackList
open Microsoft.Maui
open Fabulous.Maui
open System
open Microsoft.Maui.Controls

type IFabMediaElement =
    inherit IFabView
    inherit IFabVisualElement

// TODO: seekTo(position: TimeSpan)
type MediaElementController() =
    let play = Event<EventHandler, EventArgs>()
    let pause = Event<EventHandler, EventArgs>()
    let stop = Event<EventHandler, EventArgs>()

    [<CLIEvent>]
    member _.Play = play.Publish

    [<CLIEvent>]
    member _.Pause = pause.Publish
     
    [<CLIEvent>]
    member _.Stop = stop.Publish

    member this.DoPlay() = play.Trigger(this, EventArgs.Empty)
    member this.DoPause() = pause.Trigger(this, EventArgs.Empty)
    member this.DoStop() = stop.Trigger(this, EventArgs.Empty)
    

// We need to implement a custom MediaElement to support the Controller
type CustomMediaElement() as this =
    inherit MediaElement()
   
    let _playHandler = EventHandler(this.CustomPlay)
    let _pauseHandler = EventHandler(this.CustomPause)
    let _stopHandler = EventHandler(this.CustomStop)
    
    let mutable _oldController: MediaElementController option = None
    member this.Controller
        with get () = _oldController
        and set (newController: MediaElementController option) =
            match newController with
            | Some controller ->
                controller.Play.AddHandler(_playHandler)
                controller.Pause.AddHandler(_pauseHandler)
                controller.Pause.AddHandler(_stopHandler)
                _oldController <- Some controller
            | None -> _oldController <- None

    member private this.CustomPlay _ _ = this.Play()
    member private this.CustomPause _ _ = this.Pause()
    member private this.CustomStop _ _ = this.Stop()

module MediaElement =
    let WidgetKey = Widgets.register<MediaElement>()
   
    let inline defineProperty<'T when 'T: equality> name (defaultValue: 'T) (setter: obj -> 'T -> unit) =
        Attributes.defineSimpleScalarWithEquality<'T> name (fun _ newValueOpt node ->
            let target = node.Target :?> BindableProperty

            match newValueOpt with
            | ValueNone -> setter target defaultValue
            | ValueSome v -> setter target v)
   
    let Controller = defineProperty "MediaElement_Controller" None (fun target value -> (target :?> CustomMediaElement).Controller <- value)
     
    let Aspect = Attributes.defineBindableEnum<Aspect> MediaElement.AspectProperty

    let ShouldAutoPlay = Attributes.defineBindableBool MediaElement.ShouldAutoPlayProperty 

    let ShouldLoopPlayback = Attributes.defineBindableBool MediaElement.ShouldLoopPlaybackProperty

    let ShouldKeepScreenOn = Attributes.defineBindableBool MediaElement.ShouldKeepScreenOnProperty
    
    let ShouldShowPlaybackControls = Attributes.defineBindableBool MediaElement.ShowsPlaybackControlsProperty

    let Source = Attributes.defineBindableWithEquality<string> MediaElement.SourceProperty

    let Speed = Attributes.defineBindableFloat MediaElement.SpeedProperty

    let Volume = Attributes.defineBindableFloat MediaElement.VolumeProperty
    
    // ---- Events -----
    
    let MediaOpened = Attributes.defineEventNoArg "MediaElement_MediaOpened" (fun target -> (target :?> MediaElement).MediaOpened)
    
    let MediaEnded = Attributes.defineEventNoArg "MediaElement_MediaEnded" (fun target -> (target :?> MediaElement).MediaEnded)
    
    let MediaFailed = Attributes.defineEvent<MediaFailedEventArgs> "MediaElement_MediaFailed" (fun target -> (target :?> MediaElement).MediaFailed)
    
    let PositionChanged = Attributes.defineEvent<MediaPositionChangedEventArgs> "MediaElement_PositionChanged" (fun target -> (target :?> MediaElement).PositionChanged)
   
    let SeekCompleted = Attributes.defineEventNoArg "MediaElement_SeekCompleted" (fun target -> (target :?> MediaElement).SeekCompleted) 
    

[<AutoOpen>]
module MediaElementBuilders =
    type Fabulous.Maui.View with
       
        /// <summary>MediaElement is a cross-platform control for playing video and audio.</summary> 
        [<Extension>] 
        static member MediaElement() =
            WidgetBuilder<'msg, IFabMediaElement>(MediaElement.WidgetKey, AttributesBundle(StackList.empty(), ValueNone, ValueNone))
        
        /// <summary>MediaElement is a cross-platform control for playing video and audio.</summary>
        /// <param name ="source">The source of the media loaded into the control.</param> 
        [<Extension>]
        static member inline MediaElement<'msg>(source: string) =
            WidgetBuilder<'msg, IFabMediaElement>(MediaElement.WidgetKey, MediaElement.Source.WithValue(source))
            
            
[<Extension>]
type MediaElementModifiers =
  
    [<Extension>]
    static member inline aspect(this: WidgetBuilder<'msg, #IFabMediaElement>, value: Aspect) =
        this.AddScalar(MediaElement.Aspect.WithValue(value))
     
    [<Extension>]
    static member inline shouldAutoPlay(this: WidgetBuilder<'msg, #IFabMediaElement>, value: bool) =
        this.AddScalar(MediaElement.ShouldAutoPlay.WithValue(value))

    [<Extension>]
    static member inline shouldLoopPlayback(this: WidgetBuilder<'msg, #IFabMediaElement>, value: bool) =
        this.AddScalar(MediaElement.ShouldLoopPlayback.WithValue(value))

    [<Extension>]
    static member inline shouldKeepScreenOn(this: WidgetBuilder<'msg, #IFabMediaElement>, value: bool) =
        this.AddScalar(MediaElement.ShouldKeepScreenOn.WithValue(value))

    [<Extension>]
    static member inline shouldShowPlaybackControls(this: WidgetBuilder<'msg, #IFabMediaElement>, value: bool) =
        this.AddScalar(MediaElement.ShouldShowPlaybackControls.WithValue(value))

    [<Extension>]
    static member inline source(this: WidgetBuilder<'msg, #IFabMediaElement>, value: string) =
        this.AddScalar(MediaElement.Source.WithValue(value))
        
    [<Extension>]
    static member inline speed(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.Speed.WithValue(value))

    [<Extension>]
    static member inline volume(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.Volume.WithValue(value))

    /// <summary>Link a ViewRef to access the direct MediaElement control instance</summary>
    /// <param name="this">Current widget</param>
    /// <param name="value">The ViewRef instance that will receive access to the underlying control</param>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IFabMediaElement>, value: ViewRef<MediaElement>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
       
    [<Extension>]
    static member inline controller(this: WidgetBuilder<'msg, #IFabMediaElement>, value: MediaElementController) =
        this.AddScalar(MediaElement.Controller.WithValue(Some value)) 
       
       
        
    // ---- Event Listeners ----
    
    /// <summary>Listen to the OnMediaOpened event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="msg">Message to dispatch</param>
    [<Extension>]
    static member inline onMediaOpened(this: WidgetBuilder<'msg, #IFabMediaElement>, msg: 'msg) =
        this.AddScalar(MediaElement.MediaOpened.WithValue(msg))
        
    /// <summary>Listen to the OnMediaOpened event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="msg">Message to dispatch</param>
    [<Extension>]
    static member inline onMediaEnded(this: WidgetBuilder<'msg, #IFabMediaElement>, msg: 'msg) =
        this.AddScalar(MediaElement.MediaEnded.WithValue(msg))
        
    /// <summary>Listen to the OnMediaFailed event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="msg">Message to dispatch</param>
    [<Extension>]
    static member inline onMediaFailed(this: WidgetBuilder<'msg, #IFabMediaElement>, fn: MediaFailedEventArgs -> 'msg) =
        this.AddScalar(MediaElement.MediaFailed.WithValue(fun args -> fn args |> box))
        
    /// <summary>Listen to the OnPositionChanged event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="msg">Message to dispatch</param>
    [<Extension>]
    static member inline onPositionChanged(this: WidgetBuilder<'msg, #IFabMediaElement>, fn: MediaPositionChangedEventArgs -> 'msg) =
        this.AddScalar(MediaElement.PositionChanged.WithValue(fun args -> fn args |> box))
    
    /// <summary>Listen to the SeekCompleted event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="msg">Message to dispatch</param>
    [<Extension>]
    static member inline onSeekCompleted(this: WidgetBuilder<'msg, #IFabMediaElement>, msg: 'msg) =
        this.AddScalar(MediaElement.SeekCompleted.WithValue(msg)) 
