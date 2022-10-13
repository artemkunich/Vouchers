export const createGetDomainDetail = (authHttpRequest, serializeToQuery) => async (token, domainId) => {
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

        const response = await authHttpRequest(`DomainDetail/${domainId}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPutDomainDetail = (authHttpRequest) => async (token, updateDomainDetailCommand) => {
    try {
        console.log(JSON.stringify(updateDomainDetailCommand))

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        //headers['Content-Type'] = 'application/json'

        const request = {
            method: 'PUT',
            headers: headers,
            body: updateDomainDetailCommand
        }

        const response = await authHttpRequest('DomainDetail', request)

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