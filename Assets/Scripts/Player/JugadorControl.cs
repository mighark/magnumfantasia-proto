using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JugadorControl : MonoBehaviour {

    //Colliders
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    
    //Colliders de habilidades
    public GameObject arma;
    public GameObject escudo;
    private Weapon armaScript;

    [SerializeField] private LayerMask groundLayerMask; //para isGrounded
    //Variables para sprites, animación y audio
    private SpriteRenderer mySpriteRenderer;
    private Animator animator;
    public AudioSource sonidoDaño;
    public AudioSource sonidoBlock;
    //Variables de HUD
    public HealthBar healthBar;
    public HealthBar manaBar;
    public StageFinish gameController;

    //Inventario
    
    //Datos de personaje
    public float speed = 8.0f;
    public float speedJump = 30.0f;
    private float currentSpeed;
    private float currentSpeedJump;
    private Vida vida;
    public int manaPerHit = 2;
    public int manaPerGetHit = 1;
    public int manaPerBlock = 1;
    public bool dead = false;

    //Variables que controlan los saltos
    private bool canControl = true;
    private bool jump = false;
    ///private bool doblejump = false;
    ///Variable que controla el estar agachado
    //private bool crouch = false;
    
    //Variables que controlan el uso de habilidades
    private bool attacking = false;
    private bool blocking = false;
    private string casting = "";
    private SkillManager skillManager;
    //private BuffManager buffManager;
    ///private bool gafas = false;
    private float jugadorScale;

    //Variables que controlan el recibir daño
    ///public float spriteBlinkingTimer = 0.0f;
    ///public float spriteBlinkingMiniDuration = 0.1f;
    ///public float spriteBlinkingTotalTimer = 0.0f;
    ///public float spriteBlinkingTotalDuration = 1.0f;
    ///public bool dañado = false;

    void Start () {
        //Obtener componentes
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        armaScript = arma.GetComponent<Weapon>();

        //Componentes audiovisual
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //Inicializar variables internas
        vida = GetComponent<Vida>();
        currentSpeed = speed;
        currentSpeedJump = speedJump;

        jugadorScale = transform.localScale.x;

        //Modo dios
        //Cheat();
        manaBar.UpdateBar(vida.currentMana, vida.maxMana);
        skillManager = GetComponent<SkillManager>();
        //buffManager = GetComponent<BuffManager>();
        vida.onHitEffect += onHitEffect; 
        
    }
    
    void OnEnable() {
        if(vida) {
            vida.onHitEffect += onHitEffect; 
        }
    }
    
    void OnDisable() {
        vida.onHitEffect -= onHitEffect; 
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Time.timeScale == 1) { //Si el juego no está en pausa

            //Obtienes el input
            float horizontalInput = Input.GetAxis("Horizontal");
            bool jumpInput = Input.GetButtonDown("Jump"); 
            ///float crouchInput = Input.GetAxisRaw("Crouch");
            bool ataqueInput = Input.GetButtonDown("Attack");
            bool blockInput = Input.GetButtonDown("Block");
            bool[] skillInput = new bool[6];

            for(int i = 0; i < skillInput.Length; i++) {
                skillInput[i] = Input.GetButtonDown("Skill" + i);
            }
        
            //Controla el giro del personaje y de los colliders de sus habilidades
            if(checkControl()){
                if (horizontalInput < 0){
                    transform.localScale = new Vector3(-jugadorScale, transform.localScale.y, transform.localScale.z);
                }else if (horizontalInput > 0){
                    transform.localScale = new Vector3(jugadorScale, transform.localScale.y, transform.localScale.z);
                }
            }

            //Controla el salto
            if (jumpInput && !jump && isGrounded() && checkControl()) {
                jump = true;
                animator.SetBool("Jump", true);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, speedJump), ForceMode2D.Impulse);
            } else if (jump && rb.velocity.y <= 0 && isGrounded()){
                jump = false;
                ///doblejump = false;
                animator.SetBool("Jump", false);
            }

            //Controla el agacharse
            ///if ((crouchInput == 1) && isGrounded() && !attacking){
                ///crouch = true;
                ///crouchCollider.enabled = true;
                ///bc.enabled = false;
            ///}else{
                ///crouch = false;
                ///crouchCollider.enabled = false;
                ///bc.enabled = true;
            ///}

            ///animator.SetBool("Crouch", crouch);

            //Comprueba el uso y cambio de habilidades
            comprobarAtaque(ataqueInput);
            comprobarBlock(blockInput);
            comprobarSkill(skillInput);

            //Controla el movimiento
            if (checkControl()){
                animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
                ///if(crouch){
                ///    rb.velocity = new Vector2(horizontalInput * (speed / 2), rb.velocity.y);
                ///}else{
                    rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
                ///}
            } else {
                animator.SetFloat("Speed", 0);
                if(canControl) {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }

            //Controla la muerte
            if(vida.currentHealth <= 0 && !dead){
                dead = true;
                animator.SetTrigger("Dead");
                gameObject.layer = LayerMask.NameToLayer("Dead Units");
                Invoke("die", 0.8f);
            }
        }
        
    }

    public bool isGrounded(){
        RaycastHit2D ray;
        ray = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.1f, groundLayerMask);
        return ray.collider != null;
    }
    
    public bool checkControl() {
        return canControl && !attacking && !blocking && !dead && casting == "";
    }

    //Controla el uso de habilidades
    void comprobarAtaque(bool ataqueInput){
        if(ataqueInput && checkControl() && isGrounded()){
            attacking = true;
            animator.SetTrigger("Attack");
            ///audioSource[1].PlayDelayed(0.05f);
            ///Invoke("ataqueOn", 0.15f);
        }
    }

    void comprobarBlock(bool blockInput){
        if(blockInput && checkControl() && isGrounded()){
            blocking = true;
            animator.SetTrigger("Block");
            escudo.SetActive(true);
        }
    }
    
    void comprobarSkill(bool[] skillInput) {
        if(checkControl()) {
            for(int i = 0; i < skillInput.Length; i++) {
                if(skillInput[i]) {
                    //if skill not on cd
                    if(skillManager.canCastSkill(i)) {
                        Skill skill = skillManager.skills[i];
                        //if jumping, only use skill if possible
                        if(skill.canUseJumping || isGrounded()) {
                            //if have enough mana
                            if(vida.currentMana >= skill.manaCost) {
                                spendMana(skill.manaCost);
                                skillManager.cooldownSkill(i);
                                skill.attemptSkill(vida);
                                //if cast required, do cast
                                if(skill.castTime > 0) {
                                    casting = skill.castAnim;
                                    animator.SetBool(casting, true);
                                    Invoke("endCast", skill.castTime);
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
    
    void endCast() {
        animator.SetBool(casting, false);
        casting = "";
    }
    
    public bool isFacingRight() {
        return transform.localScale.x < 0;
    }
    
    public void takeDamage(EnemyWeapon attack) {
        if(blocking) {
            if(isInFront(attack.transform.position.x)) {
                sonidoBlock.Play();
                gainMana(manaPerBlock);
                return;
            }
        }
        vida.damageToHealth(attack.getDamage(vida));
        attack.applyKnockback(transform);
        //vida.critReset();
        gainMana(manaPerGetHit);
        damageTaken();
    }
    
    public bool isInFront(float xPosition) {
        if(transform.position.x - xPosition > 0) {
            return !isFacingRight();
        } else {
            return isFacingRight();
        }
    }
    
    private void damageTaken(){
        if(!dead) {
            //feedback visual y mercy invincibility
            healthBar.UpdateBar(vida.currentHealth, vida.maxHealth);
            mySpriteRenderer.color = Color.red;
            animator.SetTrigger("Hurt");
            if(attacking)
                ataqueOff();
            if(blocking)
                blockOff();
            sonidoDaño.Play();
        }
    }
        
    private void knockback(float dir) {
        CancelInvoke("returnControl");
        canControl = false;
        rb.AddForce(new Vector2(dir, 0), ForceMode2D.Impulse);
        Invoke("returnControl", 0.4f);
    }
    
    public void shortKnockback(float dir) {
        if(canControl) {
            CancelInvoke("returnControl");
            canControl = false;
            Invoke("returnControl", 0.1f);
        }
        rb.AddForce(new Vector2(dir, 0), ForceMode2D.Impulse);
    }
    
    private void returnControl() {
        canControl = true;
        mySpriteRenderer.color = Color.white;
    }

    //Controla el proceso de muerte y "respawn"
    private void die(){
        gameController.gameOver();
        /*muerteTexto.SetActive(true);
        audioSource[11].Play();
        mySpriteRenderer.enabled = false;
        bc.enabled = false;
        Invoke("respawn", 5f);*/
    }
    
    private void respawn(){
        /*if(PlayerPrefs.HasKey("Escena")){
            SceneManager.LoadSceneAsync(PlayerPrefs.GetString("Escena"));
        }else{
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }*/
    }

    public void damageOverTime(int dmg) {
        /*if(!dead) {
            currentHealth -= dmg;
            cambioColor(Color.green);
            if(!audioSource[10].isPlaying){
                audioSource[10].Play();
            }
            hudVida.UpdateBar(currentHealth, maxHealth);
            CancelInvoke("ResetTos");
            Invoke("ResetTos", 0.6f);
            CancelInvoke("ResetColor");
            Invoke("ResetColor", 0.1f);
        }*/
        
    }
    
    /*public void damageInstant(int dmg) {
        currentHealth -= dmg;
        ///hudVida.UpdateBar(currentHealth, maxHealth);
    }*/

    public void damageSpeed(float speed, float speedJump){
        ///if (!dañado){
            this.speed = speed;
            this.speedJump = speedJump;
            cambioColor(Color.yellow);
            CancelInvoke("restoreSpeed");
            Invoke("restoreSpeed", 2f);
        ///}
    }

    /*public void damageEnergy(int dmg){
        ///if(!dañado){
            if (currentEnergy > 0){
                currentEnergy -= dmg;
                ///hudEnergia.UpdateBar(currentEnergy, maxEnergy);
            }
            cambioColor(Color.blue);
            CancelInvoke("ResetColor");
            Invoke("ResetColor", 0.5f);
            CancelInvoke("atacandoTimerReinicia");
            ///Invoke("atacandoTimerReinicia", atacandoTime);
        ///}
    }*/

    public void damageEnergyOverTime(int dmg){
        /*if(currentEnergy > 0){
            currentEnergy -= dmg;
            hudEnergia.UpdateBar(currentEnergy, maxEnergy);
        }
        cambioColor(Color.blue);
        if (!audioSource[10].isPlaying){
            audioSource[10].Play();
        }
        CancelInvoke("ResetTos");
        Invoke("ResetTos", 0.6f);
        CancelInvoke("ResetColor");
        Invoke("ResetColor", 0.1f);
        atacandoTimer = true;
        CancelInvoke("atacandoTimerReinicia");
        Invoke("atacandoTimerReinicia", atacandoTime);*/
    }

    public void restoreSpeed(){
        this.speed = currentSpeed;
        this.speedJump = currentSpeedJump;
        ResetColor();
    }

    //Permite recuperar vida
    /*public void recargarVida(int vida) {
        currentHealth += vida;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        ///hudVida.UpdateBar(currentHealth, maxHealth);
    }*/
    
    private void gainMana(int mana) {
        vida.gainMana(mana);
        manaBar.UpdateBar(vida.currentMana, vida.maxMana);
    }
    
    private void spendMana(int mana) {
        gainMana(-mana);
    }
    
    private void ataqueOn(){
        if(attacking) {
            arma.SetActive(true);
            armaScript.readyAttack();
        }
    }
    
    private void ataqueOff(){
        armaScript.finishAttack();
        attacking = false;
    }
    
    public void onHitEffect() {
        gainMana(manaPerHit);
    }
    
    private void blockOff(){
        blocking = false;
        escudo.SetActive(false);
    }

    private void cambioColor(Color color) {
        mySpriteRenderer.color = color;
    }

    private void ResetColor() {
        mySpriteRenderer.color = Color.white;
    }

    private void ResetTos(){
        ///audioSource[10].Stop();
    }

    //Modo dios
    /*private void Cheat(){
        //currentHealth = 10000;
        //currentEnergy = 10000;
        for (int i = 0; i < armas.Length; i++){
            armas[i] = true;
        }
        for (int i = 0; i < llaves.Length; i++){
            llaves[i] = true;
        }
    }*/

}
