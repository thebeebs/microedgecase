<?php
	require_once 'Services/Twilio.php';
	
	// try 
	// {
	// 	$client = new Services_Twilio($account_sid, $auth_token); 
	// 	$message = $client->account->messages->get("SM850d0e9055706df21fd226f84431f3b4"); 
	// 	echo $message->body;
	// } 
	// catch (Exception $e) 
	// {
	// 	echo 'Error: ' . $e->getMessage();
	// }
	
	$account_sid = 'ACdf2c6c6aecf1040591d0bf905d0663c9'; 
	$auth_token = 'd1663fdad12ad072387403c741989ce7'; 
	$client = new Services_Twilio($account_sid, $auth_token); 
	
	$message = $client->account->messages->get("SMba1e419612392ddfe4a9e97a7a570087"); 
	echo $message->body;
			
?>
		
		
