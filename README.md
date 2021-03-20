# CGML
*(current.gen Markup Language)*

A quick way to save data to text files and read it back - not fast, not perfect, but it's simple and it does what I want. Slowly adding features.

Built in C# for Unity but can be used anywhere, wiki soon if anyone wants it

**Disclaimer:** this library is undergoing large changes often and may not be (definitely isn't) ready for production. Use and update with caution. If you're still interested, keep reading.

## Here's the idea
I wanted a simple node-based system for moving data in and out of text files, so here we are. Let's go through the key ideas:
- A `Node` has a name and (optional) value, and can store attributes (think XML/HTML)
- All nodes can have child nodes, and all child nodes are just normal nodes, allowing for unlimited hierarchy levels
- Each `CGMLObject` has a single root node and the ability to recursively convert all child nodes to text

## For example
A pretty-printed settings file
```xml
<Root>
	<Audio>
		<Volume["0.8"] Sound="1" Music="1">
		</Volume>
	</Audio>
	<Display>
		<Resolution ResX="1280" ResY="720" Freq="60"></Resolution>
		<VSync["0"]></VSync>
		<Fullscreen["2"]></Fullscreen>
	</Display>
	<Graphics>
		<ShadowDistance["200"]></ShadowDistance>
		<ShadowQuality["2"]></ShadowQuality>
		<TextureQuality["2"]></TextureQuality>
		<AntiAliasing["1"]></AntiAliasing>
		<ViewDistance["2"]></ViewDistance>
	</Graphics>
	<Gameplay>
		<Difficulty["1"] EnableMonsters="true"></Difficulty>
	</Gameplay>
</Root>
```

## Unity installation
### 2020.1.3+ (optional) (strongly recommended)
1. Change to dark theme
2. Follow **2019.4+** instructions

### 2019.4+
1. Open Package Manager
2. Click the `➕▾` button
3. Click `Add package from git URL...`
4. Add `https://github.com/Alscenic/cgml.git`

### < 2019.4
1. In your project folder, open `Packages/manifest.json`
2. Add `"com.currentgenstudios.cgml": "https://github.com/Alscenic/cgml.git",` to the dependencies

### Recommended utilities
I highly recommend you use one of these UPM extensions:
- https://github.com/QuantumCalzone/UnityGitPackageUpdater
  - Simple
- https://github.com/mob-sakai/UpmGitExtension
  - More advanced
  - (use 1.3.3 instead of 1.3.4 for Unity 2020.2.X if you experience problems)

## Installing anywhere else
- Clone the repository to your project folder and update when necessary

## Etc
Email me if you have questions, concerns, suggestions, or just want to tell me how much you hate it (https://github.com/Alscenic)

All included code is © Kyle Lamothe 2021
