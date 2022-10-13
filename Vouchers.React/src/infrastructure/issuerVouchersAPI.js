export const createGetIssuerVouchers = (authHttpRequest, serializeToQuery) => async (token, query) => {
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

        const response = await authHttpRequest(`IssuerVouchers${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostIssuerVoucher = (authHttpRequest) => async (token, newVoucher) => {
    try {

        console.log(token);
        console.log(newVoucher);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newVoucher)
        }

        const response = await authHttpRequest('IssuerVouchers', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.voucherId
        }

    } catch(error) {
        console.log(error)
    }
}

export const createPutIssuerVoucher = (authHttpRequest) => async (token, voucher) => {
    try {

        console.log(token);
        console.log(voucher);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(voucher)
        }

        const response = await authHttpRequest('IssuerVouchers', request)

        if(response.status == 200)
            return await response.json();

    } catch(error) {
        console.log(error)
    }
}