export const createGetIdentityId = (authHttpRequest) => async (token) => {
    
    try {
        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest('Identity', request)
        if(response.status == 404)
            return undefined;

        const json = await response.json()

        return json.identityId

    } catch(error) {
        console.log(error)
    }
}