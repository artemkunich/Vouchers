export const createAuthHttpRequest = (vouchersApiHost, login) => async (path, request) => {
    try {
        const response = await fetch(`${vouchersApiHost}/${path}`, request)
        
        if(response.status == 401)
            login()

        if(!response.ok){
            if(response.status == 500){
                console.log(await response.text())
                //alert(await response.text())
            }
            else
                console.log(await response)
        }
        

        

        return response

    } catch(error) {
        alert(error)
    }
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