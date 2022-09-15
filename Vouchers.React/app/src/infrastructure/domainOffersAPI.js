export const createGetDomainOffers = (authHttpRequest, serializeToQuery) => async (token, query) => {
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

        const response = await authHttpRequest(`DomainOffers${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostDomainOffer = (authHttpRequest) => async (token, newDomainOffer) => {
    try {

        console.log(token);
        console.log(newDomainOffer);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newDomainOffer)
        }

        const response = await authHttpRequest('DomainOffers', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.offerId
        }

    } catch(error) {
        console.log(error)
    }
}

export const createPutDomainOffer = (authHttpRequest) => async (token, domainOffer) => {
    try {

        console.log(token);
        console.log(domainOffer);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(domainOffer)
        }

        const response = await authHttpRequest('DomainOffers', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.offerId
        }

    } catch(error) {
        console.log(error)
    }
}