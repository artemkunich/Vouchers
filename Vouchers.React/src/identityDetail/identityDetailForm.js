import React, { useState, useEffect, useCallback} from 'react'

export const IdentityDetailForm = ({detail, currentAccount, accountId, isEditState, setIsEditState, handleSetRole, handleSubmit, handleCancel}) => {

    const [formData, setFormData] = useState({
        firstName: detail.firstName,
        lastName: detail.lastName,
        isAdmin: detail.isAdmin,
        isIssuer: detail.isIssuer
    })

    useEffect(() => {
        setFormData({
            firstName: detail.firstName,
            lastName: detail.lastName,
            isAdmin: detail.isAdmin,
            isIssuer: detail.isIssuer
        })
    }, [detail])

    const handleFirstNameChange = (event) => {
        setFormData({...formData, firstName: event.target.value})
    }

    const handleLastNameChange = (event) => {
        setFormData({...formData, lastName: event.target.value})
    }

    const handleOnAdminRoleChange = useCallback(async () => {
        const newIsAdmin = !formData.isAdmin
        const result = await handleSetRole({domainAccountId: accountId ?? currentAccount?.id, isAdmin: newIsAdmin})
        if(result)
            setFormData({...formData, isAdmin: newIsAdmin})       
    }, [handleSetRole, formData])

    const handleOnIssuerRoleChange = useCallback(async () => {
        const newIsIssuer = !formData.isIssuer
        const result = await handleSetRole({domainAccountId: accountId ?? currentAccount?.id, isIssuer: newIsIssuer})
        if(result)
            setFormData({...formData, isIssuer: newIsIssuer})
    }, [handleSetRole, formData])

    return <>
        <div className="row mb-2">
            <div className="col-6">
                <label htmlFor="profilyEmail" className="form-label">Email</label>
                <input type="text" id="profilyEmail" className="form-control" value={detail.email} disabled={true}></input>               
            </div>  
        </div>
        
        <div className="row mb-2">
            <div className="col-6">
                <label htmlFor="profilyFirstName" className="form-label">First name</label>
                <input type="text" id="profilyFirstName" className="form-control" value={formData.firstName} onChange={handleFirstNameChange} disabled={!isEditState}></input>              
            </div>
            <div className="col-6">
                <label htmlFor="profilyLastName" className="form-label">Last name</label>
                <input type="text" id="profilyLastName" className="form-control" value={formData.lastName} onChange={handleLastNameChange} disabled={!isEditState}></input>                
            </div>
        </div>
        {
            currentAccount && detail.accountId ? <div className="row mt-3 mb-2">
                
                <div className="col-1 form-check ms-3" >
                    <label htmlFor={"isIssuer"} className="form-check-label">Issuer</label>
                    <input type="checkbox" id={"isIssuer"} className="form-check-input" checked={formData.isIssuer} onChange={handleOnIssuerRoleChange} disabled={!(currentAccount.isOwner || currentAccount.isAdmin)}></input> 
                </div>
                <div className="col-1 form-check">
                    <label htmlFor={"isAdmin"} className="form-check-label">Admin</label>
                    <input type="checkbox" id={"isAdmin"} className="form-check-input" checked={formData.isAdmin} onChange={handleOnAdminRoleChange} disabled={!currentAccount.isOwner}></input> 
                </div>
                <div className="col-1 form-check">
                    <label htmlFor={"isOwner"} className="form-check-label">Owner</label>
                    <input type="checkbox" id={"isOwner"} className="form-check-input" checked={detail.isOwner} onChange={() => {}} disabled={true}></input> 
                </div>                   
            </div> : <></>
        }

        <div className="row mb-2 mt-5">
            {
                isEditState ?
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" title="Save" onClick={() => handleSubmit(formData)}>Save</button>
                    </div>
                : 
                    !accountId &&
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" title="Edit" onClick={() => setIsEditState(true)}>Edit</button>
                    </div>
            }
            
            {
                (isEditState || handleCancel) && <div className="d-grid col-3">
                    <button className="btn btn-primary" title="Cancel" onClick={handleCancel}>Cancel</button>
                </div>
            }
            
        </div>
    </>
}