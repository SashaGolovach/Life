import React, { Component } from 'react';
import { Route, Switch } from "react-router-dom";
import GoogleLogin from 'react-google-login';
import {authenticationService} from "./services/authentication.service";

const About = () => (
    <div><h1>About</h1></div>
)

const NotFound = () => (
    <div><h1>NotFound</h1></div>
)

const responseGoogle = (response : any) => {
    authenticationService.loginByGoogle(response.tokenId);
}

class App extends Component {
    render() {
        return (
            <GoogleLogin
                clientId="848128067202-2p8b2tv3tfgp2o8d2psrocr4qsb8j87h.apps.googleusercontent.com"
                buttonText="Login"
                onSuccess={responseGoogle}
                onFailure={responseGoogle}
                cookiePolicy={'single_host_origin'}
            />
        );
    }
}

export default App;