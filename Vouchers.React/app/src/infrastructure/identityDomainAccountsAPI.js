export const createGetIdentityDomainAccounts = (authHttpRequest) => async (token, query) => {
    try {

        console.log(token);

        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token 
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest("IdentityDomainAccounts", request)
        
        if([400,404].includes(response.status))
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}