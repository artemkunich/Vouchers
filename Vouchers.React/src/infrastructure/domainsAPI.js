export const createGetDomains = (authHttpRequest, serializeToQuery) => async (token, query) => {
    try {

        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
        
        // headers['Content-Type'] = 'application/json'    
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`Domains${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostDomain = (authHttpRequest) => async (token, createDomainCommand) => {
    try {
        console.log(JSON.stringify(createDomainCommand))

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(createDomainCommand)
        }

        const response = await authHttpRequest('Domains', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.domainId
        }

    } catch(error) {
        console.log(error)
    }
}