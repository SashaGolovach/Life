import { BehaviorSubject } from 'rxjs';

import {handleResponse} from "../helpers/handle-response";

const currentUserSubject = new BehaviorSubject(JSON.parse(<string>localStorage.getItem('currentUser')));

export const authenticationService = {
    login,
    logout,
    loginByGoogle,
    currentUser: currentUserSubject.asObservable(),
    get currentUserValue () { return currentUserSubject.value }
};

function loginByGoogle(googleToken : string) {
    const requestOptions : RequestInit  = {
        method: 'POST',
        mode : 'cors',
        headers: { 'Content-Type': 'application/json'},
        body: JSON.stringify({ tokenId : googleToken }),
    };
    try{
        return fetch(`https://localhost:44358/auth/google`, requestOptions)
            .then(handleResponse)
            .then(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                currentUserSubject.next(user);

                return user;
            });
    }
    catch (e) {
        console.log(e);
    }
}

function login(username : string, password : string) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`https://localhost:44358/users/authenticate`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(user));
            currentUserSubject.next(user);

            return user;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    currentUserSubject.next(null);
}