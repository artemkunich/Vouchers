import React, {useState, useEffect, useRef, useCallback, useMemo } from 'react'
import {useSelector, useDispatch } from 'react-redux'
import {ListHeader, ListElement, CollapsedListElement } from '../list/listElement.js'
import { noprofileImageSrc } from '../noprofile.js';
import ReactCrop from 'react-image-crop'
import "bootstrap-icons/font/bootstrap-icons.css";

import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

export const IssuerBoard = ({api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [values, setValues] = useState([]);
    const [valuesFilter, setValuesFilter] = useState({
        ticker: "",
    });
    const [selectedValueDetail, setSelectedValueDetail] = useState()
    const [selectedVouchers, setSelectedVouchers] = useState()

    const getValues = useCallback(async query => {
        let result = await api.getValues(token, {... query, issuerDomainAccountId: currentDomainAccount.id})  
        if(!result)
            result = []
        
        setValues(result)
    }, [currentDomainAccount, token])
    const postValue = useCallback(async value => await api.postValue(token, value), [currentDomainAccount, token])
    const putValue = useCallback(async updateCommand => await api.putValue(token, updateCommand), [currentDomainAccount, token])
    const valueApi = useMemo(() => {return {
        postValue: postValue,
        putValue: putValue,
        getVouchers: async (query) => await api.getVouchers(token, query),
        postVoucher: async (newVoucher) => await api.postVoucher(token, newVoucher),
        putVoucher: async (voucher) => await api.putVoucher(token, voucher),
        postTransaction: async (transaction) => await api.postTransaction(token, {...transaction, issuerDomainAccountId: currentDomainAccount.id}),
        getValueDetail: async (valueId) => await api.getValueDetail(token, valueId)
    }}, [currentDomainAccount, token])

    useEffect(() => {

        setValuesFilter({
            ticker: "",
        })
        getValues(valuesFilter)

    }, [currentDomainAccount, token])

    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Issuer's values</h1>
            {
                selectedValueDetail ?
                    <ValueDetail valuesItem={selectedValueDetail} api={valueApi} onCancel={()=>setSelectedValueDetail()}/>  
                :
                selectedVouchers ? <Vouchers valuesItem={selectedVouchers} api={valueApi} onCancel={()=>setSelectedVouchers()}/>
                :
                <>
                    <ListElement>
                        <div className="col-3 align-middle">
                            <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                            <input type="text" id="valueTickerFilter" className="form-control" value={valuesFilter.ticker} onChange={(event) => setValuesFilter({...valuesFilter, ticker: event.target.value})}></input> 
                        </div>  
                        <div className="col-1 mb-2" > 
                            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await getValues(valuesFilter)}>Search</button>
                        </div> 
                    </ListElement>
                    <div className='row'>
                        <div className='col-9'></div>
                        <div className='col-3 d-grid' >
                            <button className="btn mb-3 ms-5 me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={() => setSelectedValueDetail({ticker: '', description: ''})}><i className="bi bi-plus-lg"/> New value</button>
                        </div>
                    </div>       
                    {          
                        values.map((value, i) => <Value key={value.id} valuesItem={value} setSelectedValueDetail={setSelectedValueDetail} setSelectedVouchers={setSelectedVouchers}/>)
                    }
                </>
            }
        </div>
    );
}

const Value = ({valuesItem, setSelectedValueDetail, setSelectedVouchers}) => {

    return <ListElement>

        <div className="col-2 align-middle">
            { valuesItem.imageBase64 && <img style={{maxHeight: 100, maxWidth: 100}} src={`data:image/png;base64,${valuesItem.imageBase64}`}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{valuesItem.ticker}</p>
            <p>{valuesItem.description}</p>
        </div> 
        <div className=" col-2">
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Edit value detail"} onClick={() => setSelectedValueDetail(valuesItem)}>
                {"Edit"}
            </button>
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 8, marginLeft: -15, width: 125}} title={"Vouchers"} onClick={() => setSelectedVouchers(valuesItem)}>
                {"Vouchers"}
            </button>
        </div>
        
    </ListElement>
}

const ValueDetail = ({valuesItem, api, onCancel}) => {

    const [detail, setDetail] = useState({
        voucherValueId: valuesItem.id,
        voucherValueDetail: {
            ticker: valuesItem.ticker,
            description: valuesItem.description
        }
    })

    const [crop, setCrop] = useState()
    const [imgSrc, setImgSrc] = useState()
    const [isCropState, setIsCropState] = useState(false)

    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    useEffect(() => {

        const effect = async () => {
            if(!valuesItem.id)
                return

            const result = await api.getValueDetail(valuesItem.id)

            console.log(JSON.stringify(result))

            setDetail({
                ...detail,
                voucherValueDetail: {
                    ticker: result.ticker,
                    description: result.description
                }
            })
            if(result.imageBase64)

            setImgSrc(`data:image/png;base64,${result.imageBase64}`)

            console.log('cropParams: ' + JSON.stringify(result.cropParameters))

            if(result.cropParameters)
                setCrop({
                    unit: '%',
                    x: result.cropParameters.x,
                    y: result.cropParameters.y,
                    keepSelection : true,
                    width: result.cropParameters.width,
                    height: result.cropParameters.height,
                })
        }

        effect()
    },[])

    const onSelectFile = async e => {
        if (e.target.files && e.target.files.length > 0) {
            setDetail({
                ...detail,
                image: e.target.files[0],
            })
            setCrop(undefined) // Makes crop preview update between images.
            const reader = new FileReader()
            reader.addEventListener('load', () => {
                setImgSrc(reader.result.toString() || '')
                setIsCropState(true)
            })
            reader.readAsDataURL(e.target.files[0])
        }
    }

    const onImageLoad = async (e) => {

        if(crop){
            return
        }

        const { width, height } = e.currentTarget

        const side = Math.min(width, height)

        const cropRelation = 66

        const percentWidth = cropRelation * side / width
        const percentHeight = cropRelation * side / height

        const percentX = (100 - percentWidth) / 2
        const percentY = (100 - percentHeight) / 2

        setCrop({
            unit: '%', // Can be 'px' or '%'
            x: percentX,
            y: percentY,
            keepSelection : true,
            width: percentWidth,
            height: percentHeight,
        })

    }

    const handleSaveCrop = async () => {
        setIsCropState(false)
    }

    const handleSubmit = async (event) => {
        const formData = new FormData();

        console.log(JSON.stringify(detail))
        console.log(JSON.stringify(detail.voucherValueDetail))

        formData.append('voucherValueDetail.ticker', detail.voucherValueDetail.ticker)
        formData.append('voucherValueDetail.description', detail.voucherValueDetail.description)

        if(detail.image)
            formData.append('image', detail.image)
        if(crop){
            formData.append('voucherValueDetail.cropParameters.x', parseFloat(crop.x))
            formData.append('voucherValueDetail.cropParameters.y', parseFloat(crop.y))
            formData.append('voucherValueDetail.cropParameters.width', parseFloat(crop.width))
            formData.append('voucherValueDetail.cropParameters.height', parseFloat(crop.height))
        }



        if(detail.voucherValueId ){
            formData.append('voucherValueId', detail.voucherValueId)

            await api.putValue(formData)           
        } else {
            if(currentDomainAccount.id) {
                formData.append('issuerDomainAccountId', currentDomainAccount.id)

                const newValueId = await api.postValue(formData)
                setDetail({
                    ...detail,
                    voucherValueId: newValueId
                })
            }
        }
    }

    return <>
        {
            isCropState ?
            <>
                  
                <div className="row mb-2">
                    <div className="col-12">
                        <ReactCrop crop={crop} onChange={(pixelCrop, percentCrop) => setCrop(percentCrop)} aspect={1}>
                            <img id='croppedImage' style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={imgSrc} onLoad={onImageLoad} />
                        </ReactCrop>
                    </div>
                </div>
                <div className="row mb-2">
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" onClick={handleSaveCrop}>Save</button>
                    </div>
                </div>
            </>
            :
            <>
                <div className="row mb-2">
                    <div className="col-6">
                        <img style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={`${imgSrc ?? noprofileImageSrc}`}></img>
                    </div>
                </div>
                <div className="row mb-2">                          
                    <div className="d-grid col-3">
                        { imgSrc && <button className="btn btn-primary" onClick={() => {setIsCropState(true); setCrop(crop)}}>Crop image</button> }
                    </div>
                    <div className="col-3">
                        <input type="file" name="Image" onChange={onSelectFile}/>
                    </div>
                </div>
            </>
                            
        }  
        <div className="row mb-2" >
            <label htmlFor="valueTicker" className="form-label">Ticker</label>
            <input type="text" id="valueTicker" className="form-control" value={detail.voucherValueDetail.ticker} onChange={async (event) => setDetail({...detail, voucherValueDetail: {...detail.voucherValueDetail, ticker: event.target.value}})}></input>                
        </div>
        <div className="row mb-2">
            <label  htmlFor="valueDescription" className="form-label">Description</label>
            <textarea id="valueDescription" className="form-control" rows="auto" value={detail.voucherValueDetail.description} onChange={async (event) => setDetail({...detail, voucherValueDetail: { ...detail.voucherValueDetail, description: event.target.value }})}></textarea>                         
        </div>
        <div className="row mb-2">
            <div className="col-2 d-grid mt-2 mb-2" >
                <button className="btn" title="Save" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={handleSubmit}>Save</button>
            </div>
            <div className="col-2 d-grid mt-2 mb-2" >
                <button className="btn" title="Cancel" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={onCancel}>Cancel</button>
            </div> 
        </div>
    </>
}

const Vouchers = ({valuesItem, api, onCancel}) => {

    const [value, setValue] = useState(valuesItem)

    const [vouchersFilter, setVouchersFilter] = useState({
        valueId: value.id,
        includePlanned: false,
        includeExpired: false,
    })

    const [vouchers, setVouchers] = useState([])

    const getVouchers = useCallback(async query => {
        console.log(JSON.stringify(query))

        let result = await api.getVouchers(query)  
        if(!result)
            result = []

        setVouchers(result)
    }, [api, vouchers])

    const postVoucher = useCallback(async voucher => {
        const voucherId = await api.postVoucher({voucherValueId: value.id, voucher: voucher})
        if(voucherId)
            await getVouchers(vouchersFilter) 
    }, [api, getVouchers, vouchersFilter])

    const voucherApi = useMemo(() => { return {
        putVoucher: async voucher => await api.putVoucher({voucherValueId: value.id, voucher: voucher}),
        postTransaction: api.postTransaction
    }}, [api])

    useEffect(() => {
        const effect = async () => await getVouchers(vouchersFilter)
        effect()
    }, [])

    return <>
        <div className="col-4 align-middle"><p className="fs-5">{value.ticker} vouchers</p></div>
        <div className="container">           
            <div className="row mt-2">
                <ListElement>
                    <div className="col-3 align-middle form-check">
                        <label htmlFor={`voucherIncludePlannedFilter_${value.id}`} className="form-check-label">Include planned</label>
                        <input type="checkbox" id={`voucherIncludePlannedFilter_${value.id}`} className="form-check-input" checked={vouchersFilter.includePlanned} onChange={() => setVouchersFilter({...vouchersFilter, includePlanned: !vouchersFilter.includePlanned})}></input> 
                    </div> 
                    <div className="col-3 align-middle form-check">
                        <label htmlFor={`voucherIncludeExpiredFilter_${value.id}`} className="form-check-label">Include expired</label>
                        <input type="checkbox" id={`voucherIncludeExpiredFilter_${value.id}`} className="form-check-input" checked={vouchersFilter.includeExpired} onChange={() => setVouchersFilter({...vouchersFilter, includeExpired: !vouchersFilter.includeExpired})}></input> 
                    </div>  
                    <div className="col-1 d-grid mt-2 mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await getVouchers(vouchersFilter)}>Search</button>
                    </div>  
                    <div className="col-1 d-grid mt-2 mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={onCancel}>Cancel</button>
                    </div>  
                </ListElement>
            </div>
            <div className="row mb-2">
                <NewVoucher postVoucher={postVoucher}/>
            </div>
            {              
                vouchers.map((voucher, i) => <div key={voucher.id} className="row mb-2"><Voucher vouchersItem={voucher} api={voucherApi}/></div>)
            }
        </div>
    </>
}

const NewVoucher = ({postVoucher}) => {

    const [newVoucher, setNewVoucher] = useState({
        validFrom: formatDate(new Date()),
        validTo: undefined,
        canBeExchanged: true
    })  

    return <ListHeader>
        <div className="col-4 align-middle"><p className="fs-5">New voucher</p></div>
        <div className="container">
            <div className="row mb-3">
                <div className="col-3" >
                    <label htmlFor="voucherValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="voucherValidFrom" className="form-control" value={newVoucher.validFrom} onChange={async (event) => setNewVoucher({...newVoucher, validFrom: event.target.value})}></input>                
                </div> 
                <div className="col-3" >
                    <label htmlFor="voucherValidTo" className="form-label">Valid to</label>
                    <input type="date" id="voucherValidTo" className="form-control" value={newVoucher.validTo} onChange={async (event) => setNewVoucher({...newVoucher, validTo: event.target.value})}></input>                
                </div> 
                <div className="col-3 align-middle form-check" >
                    <label htmlFor="voucherCanBeExchanged" className="form-check-label">Can be exchanged</label>
                    <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={newVoucher.canBeExchanged} onChange={async () => setNewVoucher({...newVoucher, canBeExchanged: !newVoucher.canBeExchanged})}></input>                
                </div>
                <div className="col-3">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await postVoucher(newVoucher)}><i className="bi bi-save"></i></button>
                </div>
            </div>                   
        </div>
    </ListHeader>
}

const Voucher = ({vouchersItem, api}) => {

    const [voucher, setVoucher] = useState(vouchersItem)
    const [issuerQuantity, setIssuerQuantity] = useState(1)
    const [redeemQuantity, setRedeemQuantity] = useState(voucher.balance)
    const setRedeemQuantityWithValidation = (quantity) => {

        if(quantity > voucher.balance)
            quantity = voucher.balance

        setRedeemQuantity(quantity)          
    }

    useEffect(() => {
        setRedeemQuantity(voucher.balance)
    },[voucher])

    const processIssuerTransaction = useCallback(async (transaction) => {
        const transactionId = await api.postTransaction(transaction)
        transactionId && setVoucher({
            ...voucher,
            balance: voucher.balance + transaction.quantity,
            supply: voucher.supply + transaction.quantity
        })
    },[voucher])

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{voucher.validFrom.substring(0,10).replaceAll('-', '.')}-{voucher.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
            <div className="col-2 align-middle"><p className="fs-5">{voucher.balance}/{voucher.supply}</p></div>
            <div className="col-6 align-middle"><p className="fs-5">{voucher.canBeExchanged ? 'Exchangeable' : ''}</p></div>
        </>
        <div className="container">           
            <div className="row mb-3">
                <div className="col-4">
                    <label htmlFor="toIssue" className="form-label">Issue</label>
                    <input type="number" id="toIssue" className="form-control" value={issuerQuantity} onChange={async (event) => setIssuerQuantity(parseInt(event.target.value))}></input>           
                </div>
                <div className="col-1">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Issue" onClick={async () => await processIssuerTransaction({voucherId: voucher.id, quantity: issuerQuantity})}><i className="bi bi-arrow-up"></i></button>
                </div>
                <div className="col-4">
                    <label htmlFor="toRedeem" className="form-label">Redeem</label>
                    <input type="number" id="toRedeem" className="form-control" value={redeemQuantity} onChange={async (event) => setRedeemQuantityWithValidation(parseInt(event.target.value))}></input>             
                </div>
                <div className="col-3">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Redeem" onClick={async () => await processIssuerTransaction({voucherId: voucher.id, quantity: -redeemQuantity})}><i className="bi bi-arrow-down"></i></button>
                </div>
            </div>

            <div className="row mb-3">
                <div className="col-3" >
                    <label htmlFor="voucherValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="voucherValidFrom" className="form-control" value={voucher.validFrom.substring(0,10)} onChange={async (event) => setVoucher({...voucher, validFrom: event.target.value})}></input>                
                </div> 
                <div className="col-3" >
                    <label htmlFor="voucherValidTo" className="form-label">Valid to</label>
                    <input type="date" id="voucherValidTo" className="form-control" value={voucher.validTo.substring(0,10)} onChange={async (event) => setVoucher({...voucher, validTo: event.target.value})}></input>                
                </div> 
                <div className="col-3 align-middle form-check" >
                    <label htmlFor="voucherCanBeExchanged" className="form-check-label">Can be exchanged</label>
                    <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={voucher.canBeExchanged} onChange={async () => setVoucher({...voucher, canBeExchanged: !voucher.canBeExchanged})}></input>                
                </div>
                <div className="col-3">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await api.putVoucher(voucher)}><i className="bi bi-save"></i></button>
                </div>
            </div>                   
        </div>
    </CollapsedListElement>
}

const formatDate = (date) => {

    var month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day ].join('-');
}