function Main()
	log:Info("Starting LUA script")
	account:Logon("127.0.0.1",5999,"eboola","tea4two","HoneyTree","Eboola")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))		
	end
)

SetSpawnEventHandler(
	function(mob)
		log:Info(mob.Name)
		if (mob.Name == account.Character) then
			log:Info(string.format("%s spawned",account.Character))
			chat:Say("#bot spawn Brumdir")			
		end
	end
)

SetMessageEventHandler(
	function(msg)
		log:Info(msg.Message)		
	end
)

