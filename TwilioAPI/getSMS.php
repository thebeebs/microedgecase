<?php

	require_once 'Services/Twilio.php';
	
	$account_sid = 'ACdf2c6c6aecf1040591d0bf905d0663c9'; 
	$auth_token = 'd1663fdad12ad072387403c741989ce7'; 
	$client = new Services_Twilio($account_sid, $auth_token); 
	
	//$message = $client->account->messages->get("SM850d0e9055706df21fd226f84431f3b4");	
	 
	$messages = $client->account->messages;

	foreach ($messages as $message) { 		
		$_SESSION["msg-sid"] = $message->sid;	
		include 'deleteSMS.php';	
		echo $message->body;
		break;
	}
			
?>
		
		
