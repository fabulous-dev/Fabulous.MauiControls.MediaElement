namespace Fabulous.MauiControls.MediaElement

open System.Runtime.CompilerServices
open CommunityToolkit.Maui.Views
open Fabulous
open Microsoft.Maui
open Fabulous.Maui


type IFabMediaElement =
    inherit IFabView

module MediaElement =
    let WidgetKey = Widgets.register<MediaElement>()
    
    // https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement#properties
     
    let Aspect = Attributes.defineBindableEnum<Aspect> MediaElement.AspectProperty
    
    let HeightRequest = Attributes.defineBindableFloat MediaElement.HeightRequestProperty // TODO: should this be here?

    let ShouldAutoPlay = Attributes.defineBindableBool MediaElement.ShouldAutoPlayProperty 

    let ShouldLoopPlayback = Attributes.defineBindableBool MediaElement.ShouldLoopPlaybackProperty

    let ShouldKeepScreenOn = Attributes.defineBindableBool MediaElement.ShouldKeepScreenOnProperty
    
    let ShouldShowPlaybackControls = Attributes.defineBindableBool MediaElement.ShowsPlaybackControlsProperty

    let Source = Attributes.defineBindableWithEquality<string> MediaElement.SourceProperty

    let Speed = Attributes.defineBindableFloat MediaElement.SpeedProperty

    let Volume = Attributes.defineBindableFloat MediaElement.VolumeProperty
    
    let WidthRequest = Attributes.defineBindableFloat MediaElement.WidthRequestProperty // TODO: should this be here?


[<AutoOpen>]
module MediaElementBuilders =
    type Fabulous.Maui.View with

        /// <summary>The MediaElement control is a cross-platform view for... TODO: finish summary</summary>
        /// <param name ="source"></param> TODO: do we want to make source a required parameter (in C# version MediaElement works without a source set)?
        [<Extension>]
        static member inline MediaElement<'msg>(source: string) =
            WidgetBuilder<'msg, IFabMediaElement>(MediaElement.WidgetKey, MediaElement.Source.WithValue(source))
            
            
[<Extension>]
type MediaElementModifiers =
  
    [<Extension>]
    static member inline aspect(this: WidgetBuilder<'msg, #IFabMediaElement>, value: Aspect) =
        this.AddScalar(MediaElement.Aspect.WithValue(value))
     
    [<Extension>]
    static member inline heightRequest(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.HeightRequest.WithValue(value))

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
    static member inline speed(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.Speed.WithValue(value))

    [<Extension>]
    static member inline volume(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.Volume.WithValue(value))

    [<Extension>]
    static member inline widthRequest(this: WidgetBuilder<'msg, #IFabMediaElement>, value: float) =
        this.AddScalar(MediaElement.WidthRequest.WithValue(value))

    /// <summary>Link a ViewRef to access the direct MediaElement control instance</summary>
    /// <param name="this">Current widget</param>
    /// <param name="value">The ViewRef instance that will receive access to the underlying control</param>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IFabMediaElement>, value: ViewRef<MediaElement>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))