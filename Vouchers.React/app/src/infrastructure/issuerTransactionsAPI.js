export const createGetIssuerTransactions = (authHttpRequest, serializeToQuery) => async (token, query) => {
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

        const response = await authHttpRequest(`IssuerTransactions${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostIssuerTransaction = (authHttpRequest) => async (token, newTransaction) => {
    try {

        console.log(token);
        console.log(newTransaction);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newTransaction)
        }

        const response = await authHttpRequest('IssuerTransactions', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.issuerTransactionId
        }

    } catch(error) {
        console.log(error)
    }
}