export const createGetIssuerValues = (authHttpRequest, serializeToQuery) => async (token, query) => {
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

        const response = await authHttpRequest(`IssuerValues${serializeToQuery(query)}`, request)
        

        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostIssuerValue = (authHttpRequest) => async (token, newVoucherValue) => {
    try {

        console.log(token);
        console.log(newVoucherValue);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        //headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: newVoucherValue
        }

        const response = await authHttpRequest('IssuerValues', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.voucherValueId
        }

    } catch(error) {
        console.log(error)
    }
}

export const createPutIssuerValue = (authHttpRequest) => async (token, voucherValue) => {
    try {

        console.log(token);
        console.log(voucherValue);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        //headers['Content-Type'] = 'application/json'

        const request = {
            method: 'PUT',
            headers: headers,
            body: voucherValue
        }

        const response = await authHttpRequest('IssuerValues', request)

        if(response.status == 200)
            return await response.json();

    } catch(error) {
        console.log(error)
    }
}