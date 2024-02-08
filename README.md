# Snow
A terrible Minecraft server implementation
## Current Features
- Spawn into the server
- Basic entity system
- See other players move

## Code base
- Full NBT implementation
- Varints and Varlongs

## Who is this for?
'Snow' was made as a fun attempt to implement a minecraft server. Please, dont use it for your minecraft server. 
Its missing so many things at the moment.
Future plans are to make this work for very custom server, like custom MMOrpgs. But at the moment its not user-friendly enough for this.
If you, for some reason, want to use it for your server, keep in mind that normal spigot devs arnt used to C#, and wont be able to help you.
Using this, your all on your own to implement your server.

I recommend this implementation to people who are very techincal as you need to implement a lot yourself to even get a basic server running.

## FAQ
Q: Can I run my SMP on this?
A: No. Only the very basic features of minecraft are implemented, so you would be missing out on basicly everything.

Q: Will it support plugins
A: Sort of, current plans are to implement a way to create plugins using DLLs, Plugins for 'normal' servers (Spigot, Bukkit ect) wont be working as they are written in a different coding language and use the java Bukkit API.

Q: Will work be done to implement vanilla features into 'Snow'?
A: Probebly not. Some basic features will be provided but a full implementation of the server as it is now took monjang 10 years so catching up with that would require a lot of work.

## Performance
First of all, are these values saying this is running better then a spigot server? Not really, a spigot server is doing a LOT and I mean a LOT more then what this is doing, like caching chunks, that uses up a lot of ram, snow dousent do this yet, we dont even support chunks at the moment. So this is not even close to a fair comparason.

Avarage MSPT for a void world
0ms - 1ms (Please read above)

Ram usage:
Around 10mb  (Please read above)
