# __CGML__ 0.1.0
*(current.gen Markup Language)*

A quick way to save data to text files and read it back - not fast, not perfect, but it's simple and it does what I want. Slowly adding features.

Built in C# for Unity but can be used anywhere, wiki soon if anyone wants it

## Here's the idea
I wanted a simple node-based system for moving data in and out of text files, so here we are. Let's go through the key ideas:
- A `Node` has a name and (optional) value, and can store attributes (think XML/HTML)
- All nodes can have child nodes, and all child nodes are just normal nodes, allowing for unlimited hierarchy levels
- Each `CGMLObject` has a single root node and the ability to recursively convert all child nodes to text

## Etc
Email me if you have questions, concerns, suggestions, or just want to tell me how much you hate it kyle@alscenic.com

All included code is © Kyle Lamothe 2021
