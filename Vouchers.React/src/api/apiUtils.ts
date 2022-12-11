import { login } from './userManager'

const apiHost = 'http://localhost:6080'

export type AuthHttpRequestType = (path: string, request: RequestInit) => Promise<Response | undefined>

export const authHttpRequest = async (path: string , request: RequestInit) => {
    try {
        const response = await fetch(`${apiHost}/${path}`, request)
        
        if(response.status == 401)
            login()

        if(!response.ok){
            if(response.status == 500){
                console.log(await response.text())
                //alert(await response.text())
            }
            else
                console.log(await response)
        }
        
        return response

    } catch(error) {
        alert(error)
    }
}

export type SerializeToQueryType = <TObj extends object>(obj : TObj)  => string

export const serializeToQuery = <TObj extends object>(obj : TObj) : string  => {

    var str = [];
    for (var key in obj){
        if (!obj.hasOwnProperty(key))
            continue;

        const value = obj[key]
        if (value === undefined || value === "")
            continue;

        if (typeof value === "string" || typeof value === 'number' || typeof value === 'boolean')
            str.push(encodeURIComponent(key) + "=" + encodeURIComponent(value));
    }
    
    console.log(JSON.stringify(obj))
    console.log(`?${str.join("&")}`)


    if(str.length > 0) {              
        return `?${str.join("&")}`
    }
    
    return ''
}

export const getById = async (path: string, id: string, token: string) => {
    try {

        console.log(token);

        const headers = new Headers()
        headers.set("Accept-Language", "cs-CZ")

        if(token)
            headers.set("Authorization", "Bearer " + token)
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`${path}/${id}`, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch (error) {
        console.log(error)
    }
}

export const getByQuery = async (path: string, query: string, token: string) => {
    try {

        console.log(token);

        const headers = new Headers()
        headers.set("Accept-Language", "cs-CZ")

        if(token)
            headers.set("Authorization", "Bearer " + token)
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        JSON.stringify(query)

        const response = await authHttpRequest(`${path}${query}`, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch (error) {
        console.log(error)
    }
}

export const postJson = async (path: string, command: any, token: string) => {
    try {
        const headers = new Headers()

        if(token)
            headers.set("Authorization", "Bearer " + token)
        
        headers.set("Content-Type", "application/json")
            
        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(command)
        }

        const response = await authHttpRequest(path, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const putJson = async (path: string, command: any, token: string) => {
    try {

        const headers = new Headers()
        if(token)
            headers.set("Authorization", "Bearer " + token)

        headers.set("Content-Type", "application/json")       
            
        const request = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(command)
        }

        const response = await authHttpRequest(path, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return response.ok

    } catch(error) {
        console.log(error)
    }
}


export const postForm = async (path: string, command: FormData, token: string) => {
    try {

        const headers = new Headers()
        if(token)
            headers.set("Authorization", "Bearer " + token)
        
        headers.set("Content-Type", "application/json")
            
        const request = {
            method: 'POST',
            headers: headers,
            body: command
        }

        const response = await authHttpRequest(path, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const putForm = async (path: string, command: FormData, token: string) => {
    try {

        const headers = new Headers()
        if(token)
            headers.set("Authorization", "Bearer " + token)
            
        const request = {
            method: 'PUT',
            headers: headers,
            body: command
        }

        const response = await authHttpRequest(path, request)
        
        if(!response || [400,404].includes(response.status))
            return undefined;

        return response.ok

    } catch(error) {
        console.log(error)
    }
}

export const deleteById = async (path: string, id: string, token: string) => {
    try {

        const headers = new Headers()
        if(token)
        headers.set("Authorization", "Bearer " + token)
            
        headers.set("Content-Type", "application/json")

        const request = {
            method: 'DELETE',
            headers: headers,
            body: JSON.stringify({holderTransactionRequestId: id})
        }

        const response = await authHttpRequest('HolderTransactionRequests', request)

        if(!response || [400,404].includes(response.status))
            return undefined;

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.transactionRequestId
        }

    } catch(error) {
        console.log(error)
    }
}