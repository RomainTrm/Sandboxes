// Code from this article: https://medium.com/@dogwith1eye/a-phantom-costume-for-our-types-694aa385f37b

type Permission = interface end
type Read = inherit Permission
type Write = inherit Permission
type ReadWrite = inherit Read inherit Write

type 'a VirtualMachine when 'a :> Permission = 
    private { version : string }

module vm =
    let private createVM = { version = "3.0" }
    let createReadVM = 
        let vm: Read VirtualMachine = createVM
        vm
    let createWriteVM = 
        let vm: Write VirtualMachine = createVM
        vm
    let createReadWriteVM = 
        let vm: ReadWrite VirtualMachine = createVM
        vm

    let getVersion (vm: #Read VirtualMachine) = vm.version
    let destroy (vm: #Write VirtualMachine) = printf "poof!"

let vm1 = vm.createReadVM
let vm2 = vm.createWriteVM
let vm3 = vm.createReadWriteVM

let v1 = vm.getVersion vm1
//let v2 = vm.getVersion vm2
let v3 = vm.getVersion vm3

//vm.destroy vm1
vm.destroy vm2
vm.destroy vm3

// not accessible 
let vm4 = { version = "3.0" }

// no proper subtypes
let vm5:Permission VirtualMachine = vm1 :> Permission VirtualMachine