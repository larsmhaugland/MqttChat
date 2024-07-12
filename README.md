# MqttChat
A simple .NET WinForms chat application using the MQTT protocol. Only works on public brokers.

## Usage
When starting the program a GUID will be generated for as client ID and by default the [HiveMQ public broker](broker.hivemq.com) will be connected to as indicated by the text in the top right. The topic ```MqttChat/#``` will be also automatically be subscribed to. 

Other public brokers can be connected to by pressing the "Change" button in the top right. In the dialog box, write the hostname of the broker and the port number and press "OK".

On the left is a list of all topics that the client is subscribed to. By clicking the text of the topic that topic is selected. The selected topic is highlighted in <span style="background-color: cyan; color: black;">cyan</span>. 

When a topic is selected the user can type a message in the bottom box and press the "Send" button to send the message to that topic. The message will be sent to the broker and all clients subscribed to that topic will receive the message. 

> [!NOTE]
When sending a message, if the selected topic contains one or more wildcard characters the wildcards will be replaced with the client's GUID. For example, if the topic ```MqttChat/#``` is selected the client will send messages to the topic ```MqttChat/GUID```.
> 

New topics can be subscribed to by typing in a topic in the top left box and pressing the "Subscribe" button.	
