# CliControllers

In a lot of cases, console applications are relatively simple programs. We use them as quick and dirty throw away solutions, or as simple transaction scripts to solve straight forward problems. Occassionally, however, we need for them to be a bit more sophisticated. They need to handle different commands and all of their arguments. Writing parsers can be fun, but it's also not simple. It's not something you want to think about when you have other work to do, so you're off to the web to find a package where someone has already done it for you.

Enter CliControllers. CliControllers provides a framwork for developing console applications using a decorated controller pattern. This is very similar to, if not the same as, the pattern that Microsoft uses for MVC and Web API. As the developer of a console application, you will create controllers for each of the commands which your application will handle. You can then decoracte these controllers and the parameters of their invoke methods to take advantage of various standard CLI patterns.

**Documentation is still under construction.**
