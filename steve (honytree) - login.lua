function Main()
	log:Info("Starting LUA script")
	account:Logon("login.eqemulator.net",5999,"pjwendy","tea4twobye4now","HoneyTree","Steve")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))
		if (success) then
		
		end
	end
)