export const createGetDomainAccounts = (authHttpRequest, serializeToQuery) => async (token, query) => {
    try {

        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token 
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        JSON.stringify(query)

        const response = await authHttpRequest(`DomainAccounts${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostDomainAccount = (authHttpRequest) => async (token, domainAccount) => {
    try {

        console.log(token);
        console.log(JSON.stringify(domainAccount));

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(domainAccount)
        }

        const response = await authHttpRequest("DomainAccounts", request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPutDomainAccount = (authHttpRequest) => async (token, domainAccount) => {
    try {

        console.log(token);
        console.log(JSON.stringify(domainAccount));

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(domainAccount)
        }

        const response = await authHttpRequest("DomainAccounts", request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}