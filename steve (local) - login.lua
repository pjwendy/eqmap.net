function Main()
	log:Info("Starting LUA script")
	account:Logon("127.0.0.1",5999,"pjwendy","tea4two","HoneyTree","Steve")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))
		if (success) then
			
		end
	end
)