## MediaElement for Fabulous.MauiControls (Work In Progress)

Fabulous.MauiControls implementation of https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement

### Status

#### Summary
The most basic functionality of the `MediaElement` works well on Android and iOS (with a workaround for a bug in the CommunityToolkit on iOS - see below).

#### Done
- All (non read-only) bindable properties of the MediaElement have been implemented (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement#properties)
- Read-only bindable properties can be accessed through the reference property via a `ViewRef` (see below)

#### TODO
- Implement functionality/methods to control the media element externally/programatically i.e. from outside of the built-in UI controls of the media element (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement#methods).
- Use a `MediaSource` type instead of string for the `Source` property?
- Implement Events (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement#events)?
- Docs
- More examples

## Known Issues

Currently doesn't work as expected on iOS because of issue on iOS due to bug in Community Toolkit's MediaElement. 

In particular, when the `MediaElement` is added to the initial view, it will crash on iOS because it's expecting some initialization process that comes later in the lifecycle.

A workaround is to add the `MediaElement` later.

```fsharp
type Msg = AppLoaded
type Model = { IsAppLoaded: bool }

let init() = { IsAppLoaded = false }

let update msg model =
    match msg with
    | AppLoaded -> { model with IsAppLoaded = true }

let view model =
    Application(
        ContentPage(
            VStack() {
            if model.IsAppLoaded then
            MediaElement(...)
            }
        )
    )
    .onStart(AppLoaded)
```

We reported the issue on the Community Toolkit repository: https://github.com/CommunityToolkit/Maui/issues/959

## Using the MediaElement 

TODO: write proper docs here.
Please see the `HelloMediaElement` sample app to see a working example.

### Accessing read-only bindable properties

You can access read-only bindable properties of the media element as follows:

```fsharp
let mediaElementRef = ViewRef<MediaElement>()

let update msg model =
    | Msg ->
    mediaElementRef.Value.Duration // Here you will get the read-only value
    model


let view model =
    VStack() {
        MediaElement(...)
        .reference(mediaElementRef)
    }
```

### Accessing the `widthRequest` and `heightRequest` properties

These have just been renamed `width` and `height` in Fabulous, for convenience.

## Other useful links:
- [The official Fabulous website](https://fabulous.dev)
- [Get started](https://fabulous.dev/maui.controls/get-started)
- [Contributor Guide](CONTRIBUTING.md)

Additionally, we have the [Fabulous Discord server](https://discord.gg/bpTJMbSSYK) where you can ask any of your Fabulous related questions.

## Supporting Fabulous

The simplest way to show us your support is by giving this project and the [Fabulous project](https://github.com/fabulous-dev/Fabulous) a star.

You can also support us by becoming our sponsor on the GitHub Sponsors program.  
This is a fantastic way to support all the efforts going into making Fabulous the best declarative UI framework for dotnet.

If you need support see Commercial Support section below.

## Contributing

Have you found a bug or have a suggestion of how to enhance Fabulous? Open an issue and we will take a look at it as soon as possible.

Do you want to contribute with a PR? PRs are always welcome, just make sure to create it from the correct branch (main) and follow the [Contributor Guide](CONTRIBUTING.md).

For bigger changes, or if in doubt, make sure to talk about your contribution to the team. Either via an issue, GitHub discussion, or reach out to the team either using the [Discord server](https://discord.gg/bpTJMbSSYK).

## Commercial support

If you would like us to provide you with:

- training and workshops,
- support services,
- and consulting services.

Feel free to contact us: [support@fabulous.dev](mailto:support@fabulous.dev)
