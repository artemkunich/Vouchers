export const createAuthHttpRequest = (vouchersApiHost, login) => async (path, request) => {
    try {
        const response = await fetch(`${vouchersApiHost}/${path}`, request)
        
        if([401, 403].includes(response.status))
            console.log(response)
        
        if(response.status == 401)
            login()

        return response

    } catch(error) {
        console.log(error)
    };
}


export const serializeToQuery = (obj) => {

    var str = [];
    for (var p in obj)
      if (obj.hasOwnProperty(p) && obj[p] !== undefined && obj[p] !== "") {
        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
      }
    
      console.log(JSON.stringify(obj))
      console.log(`?${str.join("&")}`)


    if(str.length > 0) {              
        return `?${str.join("&")}`
    }
    
    return ''
}