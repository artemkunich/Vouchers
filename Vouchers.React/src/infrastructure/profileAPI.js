export const createGetIdentityDetail = (authHttpRequest) => async (token, accountId) => {
    try {

        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(accountId ? `IdentityDetail/${accountId}` : "IdentityDetail", request)
        

        if(response.status == 404)
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}

export const createPostIdentityDetail = (authHttpRequest) => async (token, identityDetail) => {
    try {

        console.log(token);
        console.log(identityDetail);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        //headers['Content-Type'] = 'application/json'

        const request = {
            method: 'POST',
            headers: headers,
            body: identityDetail
            //body: JSON.stringify(identityDetail)
        }

        const response = await authHttpRequest('IdentityDetail', request)

        if(response.status == 200)
        {
            const json = await response.json();
            return json?.identityId
        }

    } catch(error) {
        console.log(error)
    }
}