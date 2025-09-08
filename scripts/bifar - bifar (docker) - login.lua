function Main()
	log:Info("Starting LUA script")
	account:Logon("172.29.179.249",5999,"bifar","tea4two","Honeytree","Bifar")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))
		if (success) then			
			-- Disabled chat: causing OutOfSession errors
			-- msg = "Hello"
			-- chat:Say(msg)	
			-- log:Info(msg)	
		end
	end
)