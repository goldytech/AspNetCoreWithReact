import {useEffect, useRef, useState} from "react";
import * as signalR from "@microsoft/signalr";
import {Notification} from "../types/notification";
// The purpose of using hook useRef
// The useRef hook is used to create a mutable ref object whose .current property is initialized to
// the given argument (in this case, null). The ref object is a way to access the properties of a DOM element in
// React.
//
// In this particular case, the signalRConnection ref is used to hold the HubConnection object
// so that it can be accessed from different parts of the component.
// Since the HubConnection object is created in the useEffect hook, it would normally be lost when the hook exits.
// By using a ref the connection object is preserved between renders and can be accessed
// from the joinGroup function.
// Using a ref in this way is similar to using a class field to store the HubConnection object,
// but it is more lightweight and easier to use.
const UINotification = () =>
{
    const [label, setLabel] = useState('');
    const [groupName, setGroupName] = useState('');
    const signalRConnection = useRef<signalR.HubConnection | null>(null);
    useEffect(()=>{
        signalRConnection.current = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5042/notificationhub")
            .withAutomaticReconnect()
            .build();
        signalRConnection.current.on('RefreshUI',(notification:Notification) => {
            console.log(notification);
            setLabel(notification.message);
        });
        signalRConnection.current.start().catch(err => console.error(err.toString()));
        // Cleanup the connection when the component is unmounted
        //return () => signalRConnection.stop().then(() => console.log('SignalR connection closed'));
        //return async (): Promise<void> => {
          //  await signalRConnection.stop();
        //};
    },[]);

    const joinGroup = () => {
        signalRConnection.current?.invoke('JoinGroup',groupName);
    }

    return (
        <div>
            <h3>{label}</h3>
            <br/>
            <input type="text" value={groupName} onChange={(e) => setGroupName(e.target.value)} />
            <button className="btn btn-info" onClick={joinGroup}>Join Group</button>
        </div>
    );
    }

export default UINotification