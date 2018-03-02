# Memory hacking for RA3 
## Camera Unlock
The camera can be unlocked and you can scroll out until the view distance is too great.
Memory:
RA3_1.12.game+8DB7B4 -> [+48] -> target value (byte)
The value is 1 by default. Changing that to 0 allows you to scroll out.

This operation has to be done everytime you start a match - it will be resetted.
A more bulletproof way would be to find the function to check the limit and allow it but no success in finding that.

## IRC

### Log in to gamespy service (properitary?)
[03/02/2018 21:36:11 (Debug)] 00CF5A00 -> CRYPT des 1 redalert3pc 1h9cq0NM : 2 4E873E44 73E41 3 om :

### Setting game name (spoofing possible)
[03/02/2018 21:36:12 (Debug)] 00CF5A00 -> NICK cWc.zokker13 8197937 127.0.0.1 peerchat.gamespy.com :

### Authenticating CDKEY?
[03/02/2018 21:36:12 (Debug)] 00CF5A00 -> CDKEY CENSOREDANDZEROFILLE 27.0.0.1 peerchat.gamespy.com :

### Join Gamespy with nickname
[03/02/2018 21:36:12 (Debug)] 00CF5A00 -> JOIN #GPG!2166  DZEROFILLE 27.0.0.1 peerchat.gamespy.com :

### Join a lobby (Casual 1)
[03/02/2018 21:36:12 (Debug)] 00CF5A00 -> MODE #GPG!2166  DZEROFILLE 27.0.0.1 peerchat.gamespy.com :

### Joining a game lobby
[03/02/2018 21:28:25 (Debug)] 00CF5A00 -> UTM KingKonG~ :MAP 1 M91h9cq0NM :BCLR/  chat.gamespy.com :
[03/02/2018 21:28:25 (Debug)] 00CF5A00 -> UTM KingKonG~ :REQ/ clanID= mplate=4 /  chat.gamespy.com :

### Building connection to joiner
[03/02/2018 21:28:25 (Debug)] 00CF5A00 -> UTM KingKonG~ :NAT NATINITED2 1317486133 cWc.zokker13 om :
### Connection to joiner estabilished
[03/02/2018 21:28:31 (Debug)] 00CF5A00 -> UTM KingKonG~ :NAT CONNDONE0 2 4E873E36  cWc.zokker13 om :

### Connections to other players and host
[03/02/2018 21:28:36 (Debug)] 00CF5A00 -> UTM KingKonG~,gas463 :NAT CONNDONE1 2 4E873E38 kker13 om :
[03/02/2018 21:28:50 (Debug)] 00CF5A00 -> UTM KingKonG~,benchik_85 :NAT CONNDONE3 2 4E873E3B 13 om :
[03/02/2018 21:29:29 (Debug)] 00CF5A00 -> UTM KingKonG~,LetsTalkPoo :NAT CONNDONE4 2 4E873E41 3 om :
[03/02/2018 21:29:39 (Debug)] 00CF5A00 -> UTM KingKonG~,Kpp93 :NAT CONNDONE5 2 4E873E44 73E41 3 om :

### Joiner leaving the game lobby
[03/02/2018 21:29:51 (Debug)] 00CF5A00 -> PART #GSP!redalert3pc!M91h9cq0NM : 2 4E873E44 73E41 3 om :

### ??
[03/02/2018 21:30:52 (Debug)] 00CF5A00 -> PONG :s P!redalert3pc!M91h9cq0NM : 2 4E873E44 73E41 3 om :

### Writing to chat
[03/02/2018 21:34:30 (Debug)] 00CF5A00 -> PRIVMSG #GPG!2166 :pep 91h9cq0NM : 2 4E873E44 73E41 3 om :

### Hosting a game lobby
[03/02/2018 21:39:32 (Debug)] 00CF5A00 -> JOIN #GSP!redalert3pc!MzhKzP9N9M  1 peerchat.gamespy.com :4cd6b8f65dca7142c6f7305382bf47b8
[03/02/2018 21:39:32 (Debug)] 00CF5A00 -> MODE #GSP!redalert3pc!MzhKzP9N9M +l 6 .zokker13 its magic joel 8f65dca7142c6f7305382bf47b8
[03/02/2018 21:39:32 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :Pings/ ,0,0,0,0,0 /official/absolute_zero_v2[1.12.6];MC=0;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;
[03/02/2018 21:39:32 (Debug)] 00CF5A00 -> UTM cWc.zokker13 :REQ/ Color=3 mplate=4 , , , , , , , , , , , , olute_zero_v2[1.12.6];MC=0;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;
[03/02/2018 21:39:33 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PN/ 0=cWc.zokker13 , , , , , , olute_zero_v2[1.12.6];MC=0;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;
[03/02/2018 21:39:33 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :Pings/ ,0,0,0,0,0 /official/absolute_zero_v2[1.12.6];MC=0;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:O:X:X:X:X:;
[03/02/2018 21:39:33 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PIDS/ 1be82231, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:O:X:X:X:X:;

### Changing map (to Casual Encounter)
[03/02/2018 21:42:31 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :Pings/ ,0,0,0,0,0 /official/map_mp_2_feasel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:42:32 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change options (disable VOIP)
[03/02/2018 21:43:13 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PN/ 0=RobinSparkles , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:43:13 (Debug)] 00CF5A00 -> NOTICE #GSP!redalert3pc!Mll433aa4M :Type,GUI:RuleChangeWarning,7 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change options (enable bonus crates)
[03/02/2018 21:43:42 (Debug)] 00CF5A00 -> NOTICE #GSP!redalert3pc!Mll433aa4M :Type,GUI:RuleChangeWarning,6 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 1 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change options (disallow commentary)
[03/02/2018 21:44:07 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 0 10 1 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:44:07 (Debug)] 00CF5A00 -> NOTICE #GSP!redalert3pc!Mll433aa4M :Type,GUI:RuleChangeWarning,4 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 0 10 1 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change options (allow broadcast match)
[03/02/2018 21:44:35 (Debug)] 00CF5A00 -> NOTICE #GSP!redalert3pc!Mll433aa4M :Type,GUI:RuleChangeWarning,3 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 1 0 0 1 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:44:39 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  e,GUI:RuleChangeWarning,3 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 1 0 0 1 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:

### Change options (money, 10k -> 15k)
[03/02/2018 21:45:16 (Debug)] 00CF5A00 -> NOTICE #GSP!redalert3pc!Mll433aa4M :Type,GUI:RuleChangeWarning,2 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 15000 0 0 0 0 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:45:19 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  e,GUI:RuleChangeWarning,2 , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 15000 0 0 0 0 0 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change faction to empire 
[03/02/2018 21:45:51 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :SL/ M=283data/maps/official/map_mp_2_feasel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,2,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:45:51 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,2,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change faction to Allies
[03/02/2018 21:46:13 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,4,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change faction to random
[03/02/2018 21:47:13 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :SL/ M=283data/maps/official/map_mp_2_feasel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:47:13 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change faction to soviets
[03/02/2018 21:47:35 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ PlayerTemplate=8 a1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,7,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:47:35 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change faction to commentator
[03/02/2018 21:54:46 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Color=-1 plate=3 a1b613, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,1,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;
[03/02/2018 21:54:46 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PN/ 0=RobinSparkles , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,1,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;
[03/02/2018 21:54:46 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,3,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;
[03/02/2018 21:54:48 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,3,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;

### Change faction to observer
[03/02/2018 21:55:18 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Color=-1 plate=1 a1b613, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,3,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;
[03/02/2018 21:55:18 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , ero_v2[1.12.6];MC=0;MS=0;SD=-204987693;GSID=7250;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,1,-1,-1,0,1,-1,:O:X:X:X:X:; -1,0,-1:X:X:; 1,-1,:X:X:X:X:;

### Change Team to 1
[03/02/2018 21:49:36 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Team=0  :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,-1,0,1,-1,:O:X:X:X:X:;  T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:49:37 (Info)] OK.. invoke alt keypress after ra3 has gained focus
[03/02/2018 21:49:37 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,0,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:49:40 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,0,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change Team to 3
[03/02/2018 21:49:56 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :Pings/ ,0,0,0,0,0 /official/map_mp_2_feasel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,2,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:49:57 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,2,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:50:00 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,2,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change Team to 2
[03/02/2018 21:50:19 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Team=1  :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,2,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:50:19 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:50:20 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change color to blue
[03/02/2018 21:50:57 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Color=0 :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,-1,8,-1,1,0,1,-1,:O:X:X:X:X:;   T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:50:57 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,0,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:51:00 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,0,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change color to dark green
[03/02/2018 21:51:20 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :SL/ M=283data/maps/official/map_mp_2_feasel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,2,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:51:20 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,2,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:51:20 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,2,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Change color to teal
[03/02/2018 21:51:39 (Debug)] 00CF5A00 -> UTM RobinSparkles :REQ/ Color=6 :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,2,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:51:39 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :PIDS/ 3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,6,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:51:40 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!Mll433aa4M :KPA/  3ba1b613, , , , , , , , , , , , easel7;MC=1A1CEE08;MS=0;SD=1568245594;GSID=2173;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,6,8,-1,1,0,1,-1,:O:X:X:X:X:;    T,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Someone joins
[03/02/2018 21:40:16 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PN/ 0=cWc.zokker13,1=BeBzi  , , , , , ao1;MC=8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:O:X:X:X:X:; X:;
[03/02/2018 21:40:16 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PIDS/ 1be82231, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,-1,7,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:20 (Debug)] 00CF5A00 -> UTM BeBzi :NAT/ NATHOST0 1163174581 cWc.zokker13 ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,-1,7,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:21 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :Pings/ ,,0,0,0,0 s/official/map_mp_2_rao1;MC=8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:21 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PIDS/ 1be82231, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:21 (Debug)] 00CF5A00 -> UTM cWc.zokker13,BeBzi :NAT/ NEGO0 1 4554A2B6 1, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:23 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :KPA/  54A2B6 1, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,FT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:24 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PIDS/ 1be82231, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,TT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:24 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :SL/ M=283data/maps/official/map_mp_2_rao1;MC=8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,TT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:24 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :PIDS/ 1be82231, ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,TT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:26 (Debug)] 00CF5A00 -> UTM cWc.zokker13,BeBzi :NAT CONNDONE1 0 4554A2B6 ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,TT,5,4,-1,-1,0,1,-1,:X:X:X:X:;
[03/02/2018 21:40:33 (Debug)] 00CF5A00 -> UTM #GSP!redalert3pc!MzhKzP9N9M :KPA/   4554A2B6 ,3ba031b4, , , , , , , , , , 8B444C39;MS=0;SD=1080960369;GSID=4BC4;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,3,4,-1,-1,0,1,-1,:H,83E33712,8088,TT,5,4,-1,-1,0,1,-1,:X:X:X:X:;

### Closing hosted hame lobby
[03/02/2018 21:54:03 (Debug)] 00CF5A00 -> PART #GSP!redalert3pc!Mll433aa4M : A/   game host has reset the rules to their defaults =0;MS=0;SD=-656177614;GSID=5224;GT=-1;PC=-1;RU=3 100 10000 0 1 10 0 1 0 -1 0 -1 -1 1 ;S=H,577B2AF3,0,TT,6,8,-1,-1,0,1,-1,:O:X:X:X:X:;  -1,0,-1:X:X:; 1,-1,:X:X:X:X:;