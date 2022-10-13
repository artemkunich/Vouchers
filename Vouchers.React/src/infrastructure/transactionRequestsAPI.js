

export const createGetTransactionRequests = (authHttpRequest, serializeToQuery) => async (token, query) => {
    try {
        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`HolderTransactionRequests${serializeToQuery(query)}`, request)
        

        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createGetTransactionRequest = (authHttpRequest) => async (token, transactionRequestId) => {
    try {
        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`HolderTransactionRequest/${transactionRequestId}`, request)
        

        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createGetDomainValues = (authHttpRequest, serializeToQuery) => async (token, query) => {
    try {
        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`DomainValues${serializeToQuery(query)}`, request)
        

        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostTransactionRequest = (authHttpRequest) => async (token, newTransactionRequest) => {
    try {

        console.log(token);
        console.log(JSON.stringify(newTransactionRequest));

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newTransactionRequest)
        }

        const response = await authHttpRequest('HolderTransactionRequests', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.holderTransactionRequestId
        }

    } catch(error) {
        console.log(error)
    }
}

export const createDeleteTransactionRequest = (authHttpRequest) => async (token, transactionRequestId) => {
    try {

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'DELETE',
            headers: headers,
            body: JSON.stringify({holderTransactionRequestId: transactionRequestId})
        }

        const response = await authHttpRequest('HolderTransactionRequests', request)

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