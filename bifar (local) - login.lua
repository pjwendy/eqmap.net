function Main()
	log:Info("Starting LUA script")
	account:Logon("127.0.0.1",5999,"bifar","tea4two","HoneyTree","Bifar")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))
		if (success) then			
			msg = "Hello"
			chat:Say(msg)	
			log:Info(msg)	
		end
	end
)