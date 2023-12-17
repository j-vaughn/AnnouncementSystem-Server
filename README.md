# AnnouncementSystem-Server
A Windows C# console application that acts as a server to coordinate announcements across multiple Raspberry Pi based intercom clients

Base URL: http://localhost:8888

Query string parameters:

`k` The key to access the server
`c` What action you want the server to do
`t` is the target intercom or intercom group name
`s` is the announcement name
`a` is the action to take for a command

Examples
Play announcement `FireEvac` to intercom group `Inside`
`http://localhost:8888?k=7ujB8FxStG&c=Play&t=Inside&s=FireEvac`

Stop all announcements everywhere
`http://localhost:8888?k=7ujB8FxStG&c=cmd&a=stopall`

Stop all announcements on a particular intercom group
`http://localhost:8888?k=7ujB8FxStG&c=cmd&a=stop&t=Inside`
