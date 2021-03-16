# __CGML__ 0.1.0
*(current.gen Markup Language)*

A quick, dirty, and probably really bad XML-ish way to save data to and read text files - not fast, not perfect, but it's simple and it does what I want

Wiki soon

## Here's the idea
I wanted a simple node-based system for moving data in and out of text files, so here we are. Let's go through the key ideas:
- A `Node` has a name and (optional) value, and can store attributes (think XML/HTML)
- All nodes can have child nodes, and all child nodes are just normal nodes, allowing for unlimited hierarchy levels
- Each `CGMLObject` has a single root node and the ability to recursively convert all child nodes to text

## Etc
Built in C# for Unity but can be used anywhere

All included code is © Kyle Lamothe 2021
