import React, { Component } from 'react';
import { HubConnectionBuilder } from "@aspnet/signalr";

export class Home extends Component {

    constructor(props) {
        super(props);
        this.state = { connectedToAzureSignalR: false, messages: [] };
    }

    componentDidMount() {

        // Configuring SignalR
        const signalrConn = new HubConnectionBuilder()
            .withUrl(`/signalr`)
            .build();

        // Handler for messages from server
        signalrConn.on('message-from-server', (msg) => {
            var messages = this.state.messages;
            messages.push(msg);
            this.setState({ connectedToAzureSignalR: true, messages });
        });

        // Establishing SignalR connection
        signalrConn.start().then(
            () => {
                this.setState({ connectedToAzureSignalR: true, messages: [] });
            }, err => {
                alert(`Failed to connect to SignalR:  ${JSON.stringify(err)}`);
            });
    }

  render () {
    return (
        <div>
            <h1>{this.state.connectedToAzureSignalR ? 'Connected to Azure SignalR!' : 'Connecting...'}</h1>
            {this.state.messages.map(msg => <h5>{msg}</h5>)}
        </div>
    );
  }
}
