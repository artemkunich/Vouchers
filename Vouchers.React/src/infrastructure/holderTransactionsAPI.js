export const createGetHolderTransactions = (authHttpRequest, serializeToQuery) => async (token, query) => {
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

        const response = await authHttpRequest(`HolderTransactions${serializeToQuery(query)}`, request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostHolderTransaction = (authHttpRequest) => async (token, newTransaction) => {
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

        const response = await authHttpRequest('HolderTransactions', request)

        if(response.status == 200)
        {
            const json = await response.json();
            console.log(JSON.stringify(json))
            return json?.holderTransactionId
        }

    } catch(error) {
        alert(error)
        //console.log(error)
    }
}