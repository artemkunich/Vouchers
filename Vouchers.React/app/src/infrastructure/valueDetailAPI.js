export const createGetValueDetail = (authHttpRequest) => async (token, valueId) => {
    try {
        const headers = {}
        if(token?.access_token)
            headers['Authorization'] = "Bearer " + token.access_token
            
        const request = {
            method: 'GET',
            headers: headers,
        }

        const response = await authHttpRequest(`ValueDetail/${valueId}`, request)
        

        if(response.status == 404)
            return undefined;

        return await response.json()

    } catch(error) {
        console.log(error)
    }
}