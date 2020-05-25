import {authHeader} from "../helpers/auth-header";
import {handleResponse} from "../helpers/handle-response";

export const userService = {
    getAll
};

function getAll() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    // @ts-ignore
    return fetch(`https://localhost:44358/users`, requestOptions).then(handleResponse);
}