# ScriptServices 
 
ScriptServices is a working name for a project that aims to allow you to easily expose PowerShell scripts as RESTful endpoints, using NancyFx and convention-based routing to map scripts to inbound requests.

It is intended as a cross-platform and lightweight alternative to PowerShell remoting and its session configuration features. ([https://technet.microsoft.com/en-gb/library/hh847838.aspx]())

## Why?
Whilst PowerShell lets you easily share functionality through modules etc., sometimes it would be preferable to be able to consume such functionality without having to take on its associated dependencies (e.g. bulky API or scripting libraries - looking at you VMware!).

Other use cases include building a set of services to facilitate asynchronous communications between components involved in long running processes (e.g. inter-process notifications).

The concept of being able to expose scripts as services has bubbled around our heads for a while, but the recent discovery of [Flancy](https://github.com/toenuff/flancy) inspired us to do something more than ponder the ideas and take slightly different approach.

A key principal of ScriptServices is to enable you use your existing PowerShell debugging & testing tools whilst developing your endpoints, as such it uses simple file-based scripts to define the endpoint implementations.


## Contributing

We use and develop ScriptServices for our own use, but we would love to help make it work for you too.

* Submit a pull request for a new feature or bugfix.
* Submit a failing test for a bug.
* Make a feature request by raising an issue.
* Let us know about any bugs.
* Tell us how you're using ScriptServices!

You can reach us [@naeemkhedarun](https://twitter.com/naeemkhedarun) and [@James_Dawson](https://twitter.com/James_Dawson).
