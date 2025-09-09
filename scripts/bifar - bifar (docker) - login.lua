function Main()
	log:Info("Starting LUA script")
	account:Logon("172.29.179.249",5999,"bifar","tea4two","Honeytree","Bifar")
	log:Info("Logon sent")
end;

SetLogonResultHandler(
	function(success, reason)
		log:Info(string.format("Logon %s %s", success, reason))
		if (success) then			
			-- Wait a bit for zone connection to stabilize, then send hello message
			msg = "Hello from Lua bot!"
			chat:Say(msg)	
			log:Info(msg)	
		end
	end
)

SetMessageEventHandler(
	function(message)
		log:Info(string.format("Chat received: [%s] %s: %s", message.Channel, message.From, message.Message))
		
		-- Only respond to tells OR messages that contain the bot's name "Bifar"
		local shouldRespond = message.IsTell or string.find(string.lower(message.Message), "bifar")
		
		if shouldRespond then
			-- Respond to greetings
			if string.find(string.lower(message.Message), "hello") or string.find(string.lower(message.Message), "hi") then
				if message.IsTell then
					-- Respond to tells privately
					chat:Say("Hello " .. message.From .. "!")
				else
					-- Respond publicly only if name was mentioned
					chat:Say("Hello " .. message.From .. "!")
				end
			end
			
			-- Respond to "bot" mentions
			if string.find(string.lower(message.Message), "bot") then
				chat:Say("Yes, I'm a bot created with the EverQuest Bot Ecosystem!")
			end
		end
	end
)