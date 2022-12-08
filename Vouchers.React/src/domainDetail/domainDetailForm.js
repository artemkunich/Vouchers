import React, {useState, useEffect } from 'react'

export const DomainDetailForm = ({detail, currentAccount, isEditState, setIsEditState, handleSubmit, handleCancel}) => {

    const [formData, setFormData] = useState({
        name: detail.name,
        description: detail.description
    })

    useEffect(() => {
        setFormData({
            name: detail.name,
            description: detail.description
        })
    }, [detail])

    return <>
        <div className="row mb-2">
            <div className='col-12'>
                <label htmlFor="domainName" className="form-label">Name</label>
                <input type="text" id="domainName" className="form-control" value={formData.name} onChange={async (event) => setFormData({...formData,  name: event.target.value})} disabled={!isEditState}></input>                
            </div>
        </div>
        <div className="row mb-2">
            <div className='col-12'>
                <label  htmlFor="domainDescription" className="form-label">Description</label>
                <textarea id="domainDescription" className="form-control" rows="auto" value={formData.description} onChange={async (event) => setFormData({...formData, description: event.target.value })} disabled={!isEditState}></textarea>                         
            </div>
        </div>
        <div className="row mb-2 mt-5">
        {

            isEditState ?
                <div className="d-grid col-3" >
                    <button className="btn btn-primary" title="Save" onClick={() => handleSubmit(formData)}>Save</button>
                </div>
                : (currentAccount.isAdmin || currentAccount.isOwner) && <div className="d-grid col-3">
                    <button className="btn btn-primary" title="Edit" onClick={() => setIsEditState(true)}>Edit</button>
                </div>
        }
        {
            isEditState && <div className="d-grid col-3">
                <button className="btn btn-primary" title="Cancel" onClick={handleCancel}>Cancel</button>
            </div>
        }
        </div>
    </>
}